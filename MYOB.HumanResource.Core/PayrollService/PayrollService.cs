using System;
using MYOB.HumanResource.Core.Models;

namespace MYOB.HumanResource.Core.PayrollService
{
    public class PayrollService : IPayrollService
    {
        public PayrollResult CalculateGrossMonthlyIncome(GrossIncomeRequest grossIncomeRequest)
        {
            //null request object
            if (grossIncomeRequest == null)
            {
                throw new ArgumentNullException(nameof(grossIncomeRequest));
            }

            PayrollResult payrollResult = new PayrollResult() { EmployeeName = grossIncomeRequest.EmployeeName } ;

            if (grossIncomeRequest.AnnualSalary < 0) //negative annual salary
            {
                payrollResult.StatusCode = PayrollResultStatusCode.Fail;
            }
            else
            {
                payrollResult.StatusCode = PayrollResultStatusCode.Success;
                payrollResult.CalculatedAmount = grossIncomeRequest.AnnualSalary / 12;
            }
            return payrollResult;
        }

        public PayrollResult CalculateMonthlyIncomeTax(IncomeTaxRequest incomeTaxRequest)
        {
            //null request object
            if (incomeTaxRequest == null)
            {
                throw new ArgumentNullException(nameof(incomeTaxRequest));
            }

            PayrollResult payrollResult = new PayrollResult() { EmployeeName = incomeTaxRequest.EmployeeName };

            if (incomeTaxRequest.AnnualSalary < 0) //negative annual salary
            {
                payrollResult.StatusCode = PayrollResultStatusCode.Fail;
            }
            else
            {
                payrollResult.StatusCode = PayrollResultStatusCode.Success;
                payrollResult.CalculatedAmount = IncomeTaxCalculation(incomeTaxRequest.AnnualSalary);
            }

            return payrollResult;
        }

        public PayrollResult CalculateNetMonthlyIncome(NetIncomeRequest netIncomeRequest)
        {
            //null request object
            if (netIncomeRequest == null)
            {
                throw new ArgumentNullException(nameof(netIncomeRequest));
            }

            PayrollResult payrollResult = new PayrollResult() { EmployeeName = netIncomeRequest.EmployeeName };

            if (netIncomeRequest.AnnualSalary < 0 || //negative annual salary
                netIncomeRequest.IncomeTax > netIncomeRequest.GrossIncome ) //Income tax greater than the monthly income
            {
                payrollResult.StatusCode = PayrollResultStatusCode.Fail;
            }
            else
            {
                payrollResult.StatusCode = PayrollResultStatusCode.Success;
                payrollResult.CalculatedAmount = netIncomeRequest.GrossIncome - netIncomeRequest.IncomeTax;
            }

            return payrollResult;
        }

        #region Private Methods

        private decimal IncomeTaxCalculation(decimal annualSalary)
        {
            decimal taxAmount = 0m;
            var taxBands = new[]  //Best approach would be to get from database table
            {
                new  { Lower =0m, Upper =20000m, RatePerDollar =0m},
                new  { Lower =20000m, Upper =40000m, RatePerDollar =0.1m},
                new  { Lower =40000m, Upper =80000m, RatePerDollar =0.2m},
                new  { Lower =80000m, Upper =180000m, RatePerDollar =0.3m},
                new  { Lower =180000m, Upper =decimal.MaxValue, RatePerDollar =0.4m}
            };

            foreach (var taxBand in taxBands)
            {
                if (annualSalary > taxBand.Lower)
                {
                    var taxBaseForThisBand = Math.Min(taxBand.Upper - taxBand.Lower, annualSalary - taxBand.Lower);
                    var taxForThisBand = taxBaseForThisBand * taxBand.RatePerDollar;
                    taxAmount = taxAmount + taxForThisBand;
                }
            }

            return taxAmount / 12;

        }
        #endregion

    }
}
