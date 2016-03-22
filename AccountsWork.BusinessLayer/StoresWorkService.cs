using AccountsWork.DataAccessLayer;
using AccountsWork.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace AccountsWork.BusinessLayer
{
    public interface IStoresWorkService
    {
        IList<StoreProvenWorkSet> GetWorksList();
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

        public IList<StoreProvenWorkSet> GetWorksList()
        {
            return _worksRepository.GetAll();
        }
    }
}
