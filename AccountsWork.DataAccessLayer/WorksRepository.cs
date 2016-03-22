using AccountsWork.DomainModel;
using System.ComponentModel.Composition;

namespace AccountsWork.DataAccessLayer
{
    public interface IWorksRepository : IGenericDataRepository<StoreProvenWorkSet>
    { }
    [Export(typeof(IWorksRepository))]
    public class WorksRepository : GenericDataRepository<StoreProvenWorkSet>, IWorksRepository
    {
    }
}
