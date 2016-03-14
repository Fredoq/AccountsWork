using System.Collections.Generic;
using System.ComponentModel.Composition;
using AccountsWork.DomainModel;
using AccountsWork.DataAccessLayer;
using System;

namespace AccountsWork.BusinessLayer
{
    public interface IAccountCapexesService
    {
        IList<AccountsCapexInfoSet> GetCapexesById(int id);
        void AddCapexToAccount(AccountsCapexInfoSet newCapexForAccount);
        void DeleteCapexFromAccount(AccountsCapexInfoSet currentCapex);
    }

    [Export(typeof(IAccountCapexesService))]
    public class AccountCapexesService : IAccountCapexesService
    {
        private IAccountCapexesRepository _capexesRepository;

        [ImportingConstructor]
        public AccountCapexesService(IAccountCapexesRepository capexesRepository)
        {
            _capexesRepository = capexesRepository;
        }

        public void AddCapexToAccount(AccountsCapexInfoSet newCapexForAccount)
        {
            _capexesRepository.Add(newCapexForAccount);
        }

        public void DeleteCapexFromAccount(AccountsCapexInfoSet currentCapex)
        {
            _capexesRepository.Remove(currentCapex);
        }

        public IList<AccountsCapexInfoSet> GetCapexesById(int id)
        {
            return _capexesRepository.GetList(c => c.AccountsMainId == id);
        }
    }
}
