using AccountsWork.DomainModel;
using System.ComponentModel.Composition;

namespace AccountsWork.DataAccessLayer
{
    public interface ICapexRepository : IGenericDataRepository<CapexSet>
    { }

    [Export(typeof(ICapexRepository))]
    public class CapexRepository : GenericDataRepository<CapexSet>, ICapexRepository
    {
    }
}
