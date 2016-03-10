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
