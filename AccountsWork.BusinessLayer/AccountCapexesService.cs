using System.Collections.Generic;
using System.ComponentModel.Composition;
using AccountsWork.DomainModel;
using AccountsWork.DataAccessLayer;

namespace AccountsWork.BusinessLayer
{
    public interface IAccountCapexesService
    {
        IList<AccountsCapexInfoSet> GetCapexesById(int id);
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

        public IList<AccountsCapexInfoSet> GetCapexesById(int id)
        {
            return _capexesRepository.GetList(c => c.AccountsMainId == id);
        }
    }
}
