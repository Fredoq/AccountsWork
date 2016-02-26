using AccountsWork.DomainModel;
using System.ComponentModel.Composition;

namespace AccountsWork.DataAccessLayer
{
    public interface IStoresRepository : IGenericDataRepository<StoresSet>
    { }
    [Export(typeof(IStoresRepository))]
    public class StoresRepository : GenericDataRepository<StoresSet>, IStoresRepository
    { }
}
