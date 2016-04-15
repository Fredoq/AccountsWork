using AccountsWork.DataAccessLayer;
using AccountsWork.DomainModel;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace AccountsWork.BusinessLayer
{
    [Export(typeof(ITypesService))]
    public class TypesService : ITypesService
    {
        private readonly ITypesRepository _typesRepository;

        public IList<TypeSet> GetTypes()
        {
            return _typesRepository.GetAll();
        }

        [ImportingConstructor]
        public TypesService(ITypesRepository typesRepository)
        {
            _typesRepository = typesRepository;
        }
    }
}
