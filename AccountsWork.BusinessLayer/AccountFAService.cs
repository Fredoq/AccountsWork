using AccountsWork.DataAccessLayer;
using AccountsWork.DomainModel;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System;

namespace AccountsWork.BusinessLayer
{
    public interface IAccountFAService
    {
        IList<AccountsBudgetDetailsSet> GetFAList(int id);
        void AddFAToAccount(AccountsBudgetDetailsSet newFA);
    }

    [Export(typeof(IAccountFAService))]
    public class AccountFAService : IAccountFAService
    {
        private readonly IAccountFARepository _accountFARepository;

        [ImportingConstructor]
        public AccountFAService(IAccountFARepository accountFARepository)
        {
            _accountFARepository = accountFARepository;
        }

        public void AddFAToAccount(AccountsBudgetDetailsSet newFA)
        {
            _accountFARepository.Add(newFA);
        }

        public IList<AccountsBudgetDetailsSet> GetFAList(int id)
        {
            return _accountFARepository.GetList(a => a.AccountsMainId == id);
        }
    }
}
