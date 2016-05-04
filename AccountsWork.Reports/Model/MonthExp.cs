using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsWork.Reports.Model
{
    public class MonthExp
    {
        public DateTime MonthYear { get; set; }
        public decimal Expense { get; set; }
        public int StoreCount { get; set; }
    }
}
