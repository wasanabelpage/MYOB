using NUnit.Framework;
using MYOB.HumanResource.Core.Models;
using System;

namespace MYOB.HumanResource.Core.PayrollService
{
    public class PayrollServiceTests   //todo add more tests
    {
        private readonly PayrollService _processor;
        public PayrollServiceTests()
        {
            _processor = new PayrollService();
        }

        [TestCase(5000,60000, PayrollResultStatusCode.Success)]
        [TestCase(2500, 30000, PayrollResultStatusCode.Success)]
        [TestCase(null, null, PayrollResultStatusCode.Success)]
        [TestCase(0, 0, PayrollResultStatusCode.Success)]
        [TestCase(null, -50000, PayrollResultStatusCode.Fail)]
        public void ShouldCalculateGrossMonthlyIncome(decimal expectedAmount, decimal annualSalary, PayrollResultStatusCode expectedStatusCode)
        {
            //Arrange
            GrossIncomeRequest grossIncomeRequest = new GrossIncomeRequest() { EmployeeName ="Mary",AnnualSalary= annualSalary };
            //Act
            PayrollResult payrollResult = _processor.CalculateGrossMonthlyIncome(grossIncomeRequest);
            //Assert
            Assert.AreEqual(expectedStatusCode, payrollResult.StatusCode);
            Assert.AreEqual("Mary", payrollResult.EmployeeName);
            Assert.AreEqual(expectedAmount, payrollResult.CalculatedAmount);
        }

        [TestCase(500, 60000, PayrollResultStatusCode.Success)]
        [TestCase(null, -60000, PayrollResultStatusCode.Fail)]
        public void ShouldCalculateMonthlyIncomeTax(decimal expectedAmount, decimal annualSalary, PayrollResultStatusCode expectedStatusCode)
        {
            //Arrange
            IncomeTaxRequest incomeTaxRequest = new IncomeTaxRequest() { EmployeeName = "Mary", AnnualSalary = annualSalary };
            //Act
            PayrollResult payrollResult = _processor.CalculateMonthlyIncomeTax(incomeTaxRequest);
            //Assert
            Assert.AreEqual("Mary", payrollResult.EmployeeName);
            Assert.AreEqual(expectedStatusCode, payrollResult.StatusCode);
            Assert.AreEqual(expectedAmount, payrollResult.CalculatedAmount);
        }

        [TestCase(4500, 5000,500, PayrollResultStatusCode.Success)]
        [TestCase(null, 50, 500, PayrollResultStatusCode.Fail)]
        public void ShouldCalculateNetMonthlyIncome(decimal expectedAmount, decimal grossIncome, decimal incomeTax, PayrollResultStatusCode expectedStatusCode)
        {
            //Arrange
            NetIncomeRequest netIncomeRequest = new NetIncomeRequest() { EmployeeName = "Mary", AnnualSalary = 60000 ,GrossIncome= grossIncome, IncomeTax= incomeTax };
            //Act
            PayrollResult payrollResult = _processor.CalculateNetMonthlyIncome(netIncomeRequest);
            //Assert
            Assert.AreEqual("Mary", payrollResult.EmployeeName);
            Assert.AreEqual(expectedStatusCode, payrollResult.StatusCode);
            Assert.AreEqual(expectedAmount, payrollResult.CalculatedAmount);
        }

        [Test]
        public void ShouldThrowExceptionIfGrossIncomeRequestIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _processor.CalculateGrossMonthlyIncome(null));
            Assert.AreEqual("grossIncomeRequest", exception.ParamName);
        }

        [Test]
        public void ShouldThrowExceptionIfIncomeTaxRequestIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _processor.CalculateMonthlyIncomeTax(null));
            Assert.AreEqual("incomeTaxRequest", exception.ParamName);
        }

        [Test]
        public void ShouldThrowExceptionIfNetIncomeRequestIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _processor.CalculateNetMonthlyIncome(null));
            Assert.AreEqual("netIncomeRequest", exception.ParamName);
        }
    }
}
