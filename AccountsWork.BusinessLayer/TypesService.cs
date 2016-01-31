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
