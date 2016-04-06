using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Xml.Serialization;
using AccountsWork.DataAccessLayer;
using AccountsWork.DomainModel;
using AccountsVork.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace AccountsWork.BusinessLayer
{
    [Export(typeof(IAccountsMainService))]
    public class AccountsMainService : IAccountsMainService
    {
        private readonly IAccountsMainRepository _accountsMainRepository;
        private readonly IAccountsStatusRepository _accountsStatusRepository;

        [ImportingConstructor]
        public AccountsMainService(IAccountsMainRepository accountsMainRepository, IAccountsStatusRepository accountsStatusRepository)
        {
            _accountsMainRepository = accountsMainRepository;
            _accountsStatusRepository = accountsStatusRepository;
        }

        public IList<AccountsMainSet> GetAccountsByNumber(string number)
        {
            return _accountsMainRepository.GetList(a => a.AccountNumber.Contains(number));
        }

        public IList<AccountsMainSet> GetAllAccountsWithStores()
        {
            return _accountsMainRepository.GetAll(a => a.AccountsStoreDetailsSets);
        }

        public IList<AccountsMainSet> GetAllAccountsForStore()
        {
            return _accountsMainRepository.GetAll(a => a.AccountsStatusDetailsSets, a => a.AccountsCapexInfoSets, a => a.AccountsStoreDetailsSets);
        }

        public void RemoveAccount(AccountsMainSet resultAccount)
        {
            //var acc = _accountsMainRepository.GetSingle(a => a.Id == resultAccount.Id);
            _accountsMainRepository.Remove(resultAccount);
        }

        public int SaveAccount(AccountsMainSet account)
        {
            if (account.Id == 0)
            {
                _accountsMainRepository.Add(account);
                _accountsStatusRepository.Add(new AccountsStatusDetailsSet() { AccountMainId = account.Id, AccountStatus = Statuses.InWork, AccountStatusDate = DateTime.Now });
            }
            else
            {
                _accountsMainRepository.Update(account);
            }
            return account.Id;
        }

        public IList<AccountsMainSet> GetAllAccountsWithStoresAndCapex()
        {
            return _accountsMainRepository.GetAll(a => a.AccountsCapexInfoSets, a => a.AccountsStatusDetailsSets, a => a.AccountsStoreDetailsSets);
        }

        public AccountsMainSet GetAccountById(int id)
        {
            return _accountsMainRepository.GetSingle(a => a.Id == id);
        }

        public IList<AccountsMainSet> GetJustAccounts()
        {
            return _accountsMainRepository.GetAll();
        }

        public IList<AccountsMainSet> GetAccountsWithCapexesAndStatus()
        {
            return _accountsMainRepository.GetAll(a => a.AccountsCapexInfoSets, a => a.AccountsStatusDetailsSets);
        }
    }
}