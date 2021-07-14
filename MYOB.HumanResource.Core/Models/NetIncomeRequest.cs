using System;
using System.Collections.Generic;
using System.Text;

namespace MYOB.HumanResource.Core.Models
{
    public class NetIncomeRequest : PayrollRequestBase
    {
        public decimal GrossIncome { get; set; }
        public decimal IncomeTax { get; set; }
    }
}
