using AccountsWork.DataAccessLayer;
using System.ComponentModel.Composition;

namespace AccountsWork.BusinessLayer
{
    public interface IStoresService
    {
        bool IsStoreExist(int storeNumber);
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
    }
}
