using AccountsWork.DomainModel;
using System.ComponentModel.Composition;

namespace AccountsWork.DataAccessLayer
{
    public interface IFARepository : IGenericDataRepository<FASet>
    { }
    [Export(typeof(IFARepository))]
    public class FARepository : GenericDataRepository<FASet>, IFARepository
    {
    }
}
