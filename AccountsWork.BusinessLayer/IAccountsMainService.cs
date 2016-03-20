using System.Collections.Generic;
using AccountsWork.DomainModel;

namespace AccountsWork.BusinessLayer
{
    public interface IAccountsMainService
    {
        IList<AccountsMainSet> GetAccountsByNumber(string number);
        int SaveAccount(AccountsMainSet account);
        void RemoveAccount(AccountsMainSet resultAccount);
        IList<AccountsMainSet> GetAllAccounts();
        IList<AccountsMainSet> GetAllAccountsForStore();
    }
}