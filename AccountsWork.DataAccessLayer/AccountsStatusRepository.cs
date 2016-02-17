using AccountsWork.DomainModel;
using System.ComponentModel.Composition;

namespace AccountsWork.DataAccessLayer
{
    public interface IAccountsStatusRepository : IGenericDataRepository<AccountsStatusDetailsSet>
    {

    }
    [Export(typeof(IAccountsStatusRepository))]
    public class AccountsStatusRepository : GenericDataRepository<AccountsStatusDetailsSet>, IAccountsStatusRepository
    {
    }
}
