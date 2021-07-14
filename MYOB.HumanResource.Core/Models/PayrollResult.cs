using System;
using System.Collections.Generic;
using System.Text;

namespace MYOB.HumanResource.Core.Models
{
    public class PayrollResult
    {
        public string EmployeeName { get; set; }
        public PayrollResultStatusCode StatusCode { get; set; }
        public decimal CalculatedAmount { get; set; }
    }
}
