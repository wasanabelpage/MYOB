using System;
using MYOB.HumanResource.Core.Models;

namespace MYOB.HumanResource.Core.PayrollService
{
    public class PayrollService : IPayrollService
    {
        /// <summary>
        /// Calculate the Gross Monthly Income based on annual salary
        /// </summary>
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

        /// <summary>
        /// Calculate Income tax per month
        /// </summary>
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
                payrollResult.CalculatedAmount = TaxCalculation(incomeTaxRequest.AnnualSalary, typeof(IncomeTaxRequest));
            }

            return payrollResult;
        }

        /// <summary>
        /// Calculate Net Monthly Income
        /// </summary>
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

        /// <summary>
        /// Can call this method to calc Tax for different tax bands
        /// Open/Close Principle
        /// </summary>
        /// <param name="annualSalary"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private decimal TaxCalculation(decimal annualSalary, Type type)
        {
            decimal taxAmount = 0m;
            dynamic taxBands = new { };

            if (type == typeof(IncomeTaxRequest))  //Expandable to calculate tax for other request types
            {
                taxBands = new[]  //Best approach would be to get from database table - via a data access layer
                {
                    new  { Lower =0m, Upper =20000m, RatePerDollar =0m},
                    new  { Lower =20000m, Upper =40000m, RatePerDollar =0.1m},
                    new  { Lower =40000m, Upper =80000m, RatePerDollar =0.2m},
                    new  { Lower =80000m, Upper =180000m, RatePerDollar =0.3m},
                    new  { Lower =180000m, Upper =decimal.MaxValue, RatePerDollar =0.4m}
                };
            }
            else if (type == typeof(GrossIncomeRequest)) // Future functionality of different tax calc bands within the service
            {
                throw new ArgumentNullException(type.Name);
            }
            else
            {
                throw new ArgumentNullException(type.Name);
            }
            
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
