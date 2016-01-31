using AccountsWork.DomainModel;
using System.ComponentModel.Composition;

namespace AccountsWork.DataAccessLayer
{
    public interface ITypesRepository : IGenericDataRepository<TypeSet>
    { }
    [Export(typeof(ITypesRepository))]
    public class TypesRepository : GenericDataRepository<TypeSet>, ITypesRepository
    {
    }
}
