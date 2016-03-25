using AccountsWork.DataAccessLayer;
using AccountsWork.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Collections.ObjectModel;
using System.Linq;

namespace AccountsWork.BusinessLayer
{
    public interface IStoresWorkService
    {
        IList<StoreProvenWorkSet> GetWorksList();
        void UpdateWorks(ObservableCollection<StoreProvenWorkSet> changedWorkList);
        void AddNewWork(StoreProvenWorkSet newWork);
        IList<StoreProvenWorkSet> GetWorksList(ObservableCollection<StoresSet> accountStoresList, bool v);
        void UpdateWork(StoreProvenWorkSet work, int id);
    }

    [Export(typeof(IStoresWorkService))]
    public class StoresWorkService : IStoresWorkService
    {
        private IWorksRepository _worksRepository;

        [ImportingConstructor]
        public StoresWorkService(IWorksRepository worksRepository)
        {
            _worksRepository = worksRepository;
        }

        public void AddNewWork(StoreProvenWorkSet newWork)
        {
            _worksRepository.Add(newWork);
        }

        public IList<StoreProvenWorkSet> GetWorksList()
        {
            return _worksRepository.GetAll();
        }

        public IList<StoreProvenWorkSet> GetWorksList(ObservableCollection<StoresSet> accountStoresList, bool v)
        {
            return _worksRepository.GetList(w => accountStoresList.Any(s => s.StoreNumber == w.StoreNumber) && w.IsDone == v);
        }

        public void UpdateWork(StoreProvenWorkSet work, int id)
        {
            work.MainAccountId = id;
            _worksRepository.Update(work);
        }

        public void UpdateWorks(ObservableCollection<StoreProvenWorkSet> changedWorkList)
        {
            _worksRepository.Update(changedWorkList.ToArray());
        }
    }
}
