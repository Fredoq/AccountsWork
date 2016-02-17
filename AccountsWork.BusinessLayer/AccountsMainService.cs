using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Xml.Serialization;
using AccountsWork.DataAccessLayer;
using AccountsWork.DomainModel;
using AccountsVork.Infrastructure;

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

        public IList<AccountsMainSet> GetAccounts()
        {
            return _accountsMainRepository.GetAll();
        }

        public void AddAccount(AccountsMainSet account)
        {
            _accountsMainRepository.Add(account);
            _accountsStatusRepository.Add(new AccountsStatusDetailsSet() { AccountMainId = account.Id, AccountStatus = Statuses.InWork, AccountStatusDate = DateTime.Now });
        }
    }
}