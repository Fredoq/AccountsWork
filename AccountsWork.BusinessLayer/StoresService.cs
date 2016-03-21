using AccountsWork.DataAccessLayer;
using AccountsWork.DomainModel;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System;

namespace AccountsWork.BusinessLayer
{
    public interface IStoresService
    {
        bool IsStoreExist(int storeNumber);
        IList<StoresSet> SearchStoreNumber(string searchStoreName);
        IList<StoresSet> GetStores();
        string GetStoreName(int accountStore);
    }
    [Export(typeof(IStoresService))]
    public class StoresService : IStoresService
    {
        private IStoresRepository _storesRepository;

        [ImportingConstructor]
        public StoresService(IStoresRepository storesRepository)
        {
            _storesRepository = storesRepository;
        }

        public string GetStoreName(int accountStore)
        {
            return _storesRepository.GetSingle(s => s.StoreNumber == accountStore).StoreName;
        }

        public IList<StoresSet> GetStores()
        {
            return _storesRepository.GetAll();
        }

        public bool IsStoreExist(int storeNumber)
        {
            return _storesRepository.GetSingle(s => s.StoreNumber == storeNumber) != null;
        }

        public IList<StoresSet> SearchStoreNumber(string searchStoreName)
        {
            return _storesRepository.GetList(s => s.StoreName.ToLower().Contains(searchStoreName.ToLower()));
        }
    }
}
