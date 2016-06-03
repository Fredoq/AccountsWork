using AccountsWork.DataAccessLayer;
using AccountsWork.DomainModel;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System;

namespace AccountsWork.BusinessLayer
{
    public interface IFAService
    {
        IList<FASet> GetFAList();
    }

    [Export(typeof(IFAService))]
    public class FAService : IFAService
    {
        private readonly IFARepository _faRepository;

        [ImportingConstructor]
        public FAService(IFARepository faRepository)
        {
            _faRepository = faRepository;
        }

        public IList<FASet> GetFAList()
        {
            return _faRepository.GetAll();
        }
    }
}
