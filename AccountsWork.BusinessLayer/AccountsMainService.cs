using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Xml.Serialization;
using AccountsWork.DataAccessLayer;
using AccountsWork.DomainModel;

namespace AccountsWork.BusinessLayer
{
    [Export(typeof(IAccountsMainService))]
    public class AccountsMainService : IAccountsMainService
    {
        private readonly IAccountsMainRepository _accountsMainRepository;

        [ImportingConstructor]
        public AccountsMainService(IAccountsMainRepository accountsMainRepository)
        {
            _accountsMainRepository = accountsMainRepository;
        }

        public IList<AccountsMainSet> GetAccounts()
        {
            return _accountsMainRepository.GetAll();
        }
    }
}