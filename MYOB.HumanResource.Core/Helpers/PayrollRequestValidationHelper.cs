using System;

namespace MYOB.HumanResource.Core.Helpers
{
    public static class PayrollRequestValidationHelper
    {
        /// <summary>
        /// Helps to validate Payroll request input params only
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Bool value to indicate is valid or not</returns>
        public static bool IsValidPayrollRequest(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            bool isValid = true;

            if (args.Length == 3)
            {
                if (args[0] is null || args[1] is null || args[2] is null)
                {
                    throw new ArgumentNullException(nameof(args));
                }
                var employeeName = args[1].Trim();
                var annualSalary = args[2].Trim();

                if (string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(annualSalary)) //check arguments content is null or empty strings
                {
                    isValid = false;
                }

                if (decimal.TryParse(annualSalary, out decimal decimalAnnualSalary)) //annual salary is numeric
                {
                    if (decimalAnnualSalary < 0) //check annual salary is a positive value
                    {
                        isValid = false;
                    }
                }
                else //annual salary is not numeric
                {
                    isValid = false;
                }

            }
            else
            {
                isValid = false; //assumption - we always need 3 params to process payroll request
            }
            return isValid;
        }
    }
}
