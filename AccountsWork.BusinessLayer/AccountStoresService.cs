using AccountsWork.DataAccessLayer;
using AccountsWork.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace AccountsWork.BusinessLayer
{
    public interface IAccountStoresService
    {
        IList<StoresSet> GetAccountStoresById(int id);
        void AddStoresToAccount(ObservableCollection<AccountsStoreDetailsSet> storesForAddList);
    }

    [Export(typeof(IAccountStoresService))]
    public class AccountStoresService : IAccountStoresService
    {
        private IAccountStoresRepository _accountStoresRepository;
        private IStoresRepository _storesRepository;

        [ImportingConstructor]
        public AccountStoresService(IAccountStoresRepository accountStoresRepository, IStoresRepository storesRepository)
        {
            _accountStoresRepository = accountStoresRepository;
            _storesRepository = storesRepository;
        }

        public void AddStoresToAccount(ObservableCollection<AccountsStoreDetailsSet> storesForAddList)
        {
            _accountStoresRepository.Add(storesForAddList.ToArray());
        }

        public IList<StoresSet> GetAccountStoresById(int id)
        {
            var accStores = _accountStoresRepository.GetList(a => a.AccountsMainId == id).Select(s => s.AccountStore);
            return _storesRepository.GetList(s => accStores.Contains(s.StoreNumber));
        }
    }

}
