using AccountsWork.DomainModel;
using System.ComponentModel.Composition;

namespace AccountsWork.DataAccessLayer
{
    public interface IAccountStoresRepository : IGenericDataRepository<AccountsStoreDetailsSet>
    { }
    [Export(typeof(IAccountStoresRepository))]
    public class AccountStoresRepository : GenericDataRepository<AccountsStoreDetailsSet>, IAccountStoresRepository
    { }
}
