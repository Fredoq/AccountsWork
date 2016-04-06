using AccountsWork.DomainModel;

namespace AccountsWork.Reports.Model
{
    public class AccountsWithStatus
    {
        public AccountsMainSet Account {get;set;}
        public AccountsStatusDetailsSet Status { get; set; }
    }
}
