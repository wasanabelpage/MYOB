using System;
using System.Collections.Generic;
using System.Text;

namespace MYOB.HumanResource.Core.Models
{
    public class PayrollRequestBase
    {
        public string EmployeeName { get; set; } //future enhancement possibility : to get First Name, Last Name
        public decimal AnnualSalary { get; set; }
    }
}
