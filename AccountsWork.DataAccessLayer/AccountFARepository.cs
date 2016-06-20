using AccountsWork.DomainModel;
using System.ComponentModel.Composition;

namespace AccountsWork.DataAccessLayer
{
    public interface IAccountFARepository : IGenericDataRepository<AccountsBudgetDetailsSet>
    { }
    [Export(typeof(IAccountFARepository))]
    public class AccountFARepository : GenericDataRepository<AccountsBudgetDetailsSet>, IAccountFARepository
    {
    }
}
