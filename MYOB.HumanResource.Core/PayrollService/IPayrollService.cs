using MYOB.HumanResource.Core.Models;

namespace MYOB.HumanResource.Core.PayrollService
{
    public interface IPayrollService
    {
        /// <summary>
        /// Calculate the Gross Monthly Income based on annual salary
        /// </summary>
        PayrollResult CalculateGrossMonthlyIncome(GrossIncomeRequest grossIncomeRequest);

        /// <summary>
        /// Calculate Income tax per month
        /// </summary>
        PayrollResult CalculateMonthlyIncomeTax(IncomeTaxRequest incomeTaxRequest);

        /// <summary>
        /// Calculate Net Monthly Income
        /// </summary>
        PayrollResult CalculateNetMonthlyIncome(NetIncomeRequest netIncomeRequest);
    }
}
