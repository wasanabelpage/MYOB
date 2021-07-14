using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MYOB.HumanResource.Core.PayrollService;
using MYOB.HumanResource.Core.Helpers;
using MYOB.HumanResource.Core.Models;

namespace MYOB.HumanResource
{
    class Program
    {
        public static string functionName;
        private static IPayrollService _payslipProcessor;
        private static string _payrollFunctionName ;  

        //todo add logger

        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                 .AddSingleton<IPayrollService, PayrollService>()
                 .BuildServiceProvider();

            IConfiguration configuration = new ConfigurationBuilder()
                     .AddJsonFile("appsettings.json", true, true)
                     .Build();

            _payrollFunctionName = configuration["PayrollFunctionName"].ToLower();

            _payslipProcessor = serviceProvider.GetService<IPayrollService>();

            //wait for user input to proceed processing
            while (args.Length == 0)
            {
                WriteToConsole(false, "Please Enter Input");
                args = Console.ReadLine().Split('"', StringSplitOptions.RemoveEmptyEntries);
            }

            //Based on user input decide which function to call
            ProcessFunctions(args);

            Console.ReadLine();
        }

        #region Private Methods

        /// <summary>
        /// Control centre to decide which service to use based on input params
        /// </summary>
        /// <param name="args">User input string array</param>
        private static void ProcessFunctions(string[] args)  
        {
            try
            {
                functionName = args[0].ToLower().Trim();  // We always expect the funtion name as the first parameter
                if (functionName == _payrollFunctionName)
                {
                    if (PayrollRequestValidationHelper.IsValidPayrollRequest(args)) //use a helper class to validate inputs
                    {
                        var employeeName = args[1].Trim();
                        var annualSalary = decimal.Parse(args[2].Trim());

                        GenerateMonthlyPayslip(employeeName, annualSalary);
                    }
                    else
                    {
                        WriteToConsole(true, $"Invalid Payroll Request");
                    }
                }
                else //opportunity to extend by adding more functions
                {
                    WriteToConsole(true, $"This function {functionName} is not implemented yet!");
                }
            }
            catch (Exception ex)
            {
                WriteToConsole(true, ex.Message);
            }
        }

        /// <summary>
        /// Main function for processing the pay slip request
        /// </summary>
        /// <param name="employeeName">Name of the Employee</param>
        /// <param name="annualSalary">Annual Salary of the Employee</param>
        private static void GenerateMonthlyPayslip(string employeeName, decimal annualSalary)
        {
            try
            {
                //Future possibility - check if employee exists

                //create domain objects
                GrossIncomeRequest grossIncomeRequest = new GrossIncomeRequest() { EmployeeName = employeeName, AnnualSalary = annualSalary };
                IncomeTaxRequest incomeTaxRequest = new IncomeTaxRequest() { EmployeeName = employeeName, AnnualSalary = annualSalary };

                //Payslip calculations
                var payrollGrossIncomeResult = _payslipProcessor.CalculateGrossMonthlyIncome(grossIncomeRequest);
                var payrollIncomeTaxResult = _payslipProcessor.CalculateMonthlyIncomeTax(incomeTaxRequest);

                if (payrollGrossIncomeResult.StatusCode == PayrollResultStatusCode.Success && payrollIncomeTaxResult.StatusCode == PayrollResultStatusCode.Success)
                {
                    NetIncomeRequest netIncomeRequest = new NetIncomeRequest() {
                        EmployeeName = employeeName,
                        AnnualSalary = annualSalary,
                        GrossIncome = payrollGrossIncomeResult.CalculatedAmount,
                        IncomeTax = payrollIncomeTaxResult.CalculatedAmount
                    };
                    var payrollNetIncomeResult = _payslipProcessor.CalculateNetMonthlyIncome(netIncomeRequest);

                    //Output
                    Console.WriteLine($"Monthly Payslip for \"{employeeName}\"");
                    Console.WriteLine($"Gross Monthly Income: { payrollGrossIncomeResult.CalculatedAmount:C} ");
                    Console.WriteLine($"Monthly Income Tax: { payrollIncomeTaxResult.CalculatedAmount:C} ");
                    Console.WriteLine($"Net Monthly Income: { payrollNetIncomeResult.CalculatedAmount:C} ");
                }
                else
                {
                    WriteToConsole(true, $"Payroll generation error in calculation");
                }
            }
            catch (Exception ex)
            {
                WriteToConsole(true, $"Payroll generation error");
                throw ex;
            }
        }

        /// <summary>
        /// Write to console
        /// </summary>
        /// <param name="isError">Error message</param>
        private static void WriteToConsole(bool isError, string msg)
        {
            if (isError)
            {
                Console.BackgroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Blue;
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(msg);  //Future possibility to use Logger
        }
        #endregion

    }
}
