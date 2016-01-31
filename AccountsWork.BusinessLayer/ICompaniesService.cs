using AccountsWork.DomainModel;
using System.Collections.Generic;

namespace AccountsWork.BusinessLayer
{
    public interface ICompaniesService
    {
        IList<AccountsCompaniesSet> GetCompanies();
    }
}
