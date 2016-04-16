using AccountsWork.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsWork.Reports.Model
{
    public class StoresWithCheck
    {
        public StoresSet Store { get; set; }
        public bool Check { get; set; } = false;
    }
}
