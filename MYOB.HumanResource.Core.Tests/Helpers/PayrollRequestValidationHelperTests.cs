using NUnit.Framework;
using System;

namespace MYOB.HumanResource.Core.Helpers
{
    public class PayrollRequestValidationHelperTests
    {
        [Test]
        public void ShouldThrowExceptionIfArgsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => PayrollRequestValidationHelper.IsValidPayrollRequest(null));
            Assert.AreEqual("args", exception.ParamName);
        }

        [Test]
        public void ShouldThrowExceptionIfArgsContentIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => PayrollRequestValidationHelper.IsValidPayrollRequest(new string[] { null, null, null}));
            Assert.AreEqual("args", exception.ParamName);
        }

        [TestCase(false,new string[] {"d","f"})]
        [TestCase(false, new string[] { "d", "f","s" })]
        [TestCase(false, new string[] { "", "" ,""})]
        [TestCase(false, new string[] { "rr", "Mary", "-56" })]
        [TestCase(true, new string[] { "100", "100", "100" })]
        [TestCase(true, new string[] { "myfunction", "Mary Song", "60000" })]
        public void ShouldCheckPayrollRequestArgsValidity(bool expected, string[] args)
        {
            Assert.AreEqual(expected, PayrollRequestValidationHelper.IsValidPayrollRequest(args));
        }
    }
}
