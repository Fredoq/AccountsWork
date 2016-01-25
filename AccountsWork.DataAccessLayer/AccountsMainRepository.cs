using System.ComponentModel.Composition;
using AccountsWork.DomainModel;

namespace AccountsWork.DataAccessLayer
{
    public interface IAccountsMainRepository : IGenericDataRepository<AccountsMainSet>
    {
         
    }
    [Export(typeof(IAccountsMainRepository))]
    public class AccountsMainRepository : GenericDataRepository<AccountsMainSet>, IAccountsMainRepository
    {
        
    }
}