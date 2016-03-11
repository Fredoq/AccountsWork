using AccountsWork.DomainModel;
using System.ComponentModel.Composition;

namespace AccountsWork.DataAccessLayer
{
    public interface IAccountCapexesRepository : IGenericDataRepository<AccountsCapexInfoSet>
    { }

    [Export(typeof(IAccountCapexesRepository))]
    public class AccountCapexesRepository : GenericDataRepository<AccountsCapexInfoSet>, IAccountCapexesRepository
    {
    }
}
