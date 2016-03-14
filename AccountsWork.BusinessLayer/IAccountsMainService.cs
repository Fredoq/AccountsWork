using System.Collections.Generic;
using AccountsWork.DomainModel;

namespace AccountsWork.BusinessLayer
{
    public interface IAccountsMainService
    {
        IList<AccountsMainSet> GetAccountsByNumber(string number);
        int AddAccount(AccountsMainSet account);
    }
}