using System.Collections.Generic;
using AccountsWork.DomainModel;

namespace AccountsWork.BusinessLayer
{
    public interface IAccountsMainService
    {
        IList<AccountsMainSet> GetAccounts();
        void AddAccount(AccountsMainSet account);
    }
}