using AccountsWork.DataAccessLayer;
using AccountsWork.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsWork.BusinessLayer
{
    public interface IAccountStatusService
    {
        AccountsStatusDetailsSet GetAccountStatusById(int id);
    }

    [Export(typeof(IAccountStatusService))]
    public class AccountStatusService : IAccountStatusService
    {
        private IAccountsStatusRepository _accountsStatusRepository;

        [ImportingConstructor]
        public AccountStatusService(IAccountsStatusRepository accountsStatusRepository)
        {
            _accountsStatusRepository = accountsStatusRepository;
        }

        public AccountsStatusDetailsSet GetAccountStatusById(int id)
        {
            return _accountsStatusRepository.GetList(s => s.AccountMainId == id).LastOrDefault();
        }
    }
}
