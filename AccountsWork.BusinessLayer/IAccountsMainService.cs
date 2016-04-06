using System.Collections.Generic;
using AccountsWork.DomainModel;

namespace AccountsWork.BusinessLayer
{
    public interface IAccountsMainService
    {
        IList<AccountsMainSet> GetAccountsByNumber(string number);
        int SaveAccount(AccountsMainSet account);
        void RemoveAccount(AccountsMainSet resultAccount);
        IList<AccountsMainSet> GetAllAccountsWithStores();
        IList<AccountsMainSet> GetAllAccountsForStore();
        IList<AccountsMainSet> GetAllAccountsWithStoresAndCapex();
        AccountsMainSet GetAccountById(int id);
        IList<AccountsMainSet> GetJustAccounts();
        IList<AccountsMainSet> GetAccountsWithCapexesAndStatus();
    }
}