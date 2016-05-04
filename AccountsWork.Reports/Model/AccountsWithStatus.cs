using AccountsWork.DomainModel;
using System.Collections.Generic;

namespace AccountsWork.Reports.Model
{
    public class AccountsWithStatus
    {
        public AccountsMainSet Account {get;set;}
        public AccountsStatusDetailsSet Status { get; set; }
        public IList<StoresSet> StoresList { get; set; }
    }
}
