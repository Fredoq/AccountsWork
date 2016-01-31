using AccountsWork.DomainModel;
using System.ComponentModel.Composition;

namespace AccountsWork.DataAccessLayer
{
    public interface ICompaniesRepository : IGenericDataRepository<AccountsCompaniesSet>
    { }
    [Export(typeof(ICompaniesRepository))]
    public class CompaniesRepository : GenericDataRepository<AccountsCompaniesSet>, ICompaniesRepository
    {
    }
}
