using AccountsWork.DataAccessLayer;
using AccountsWork.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsWork.BusinessLayer
{
    [Export(typeof(ICompaniesService))]
    public class CompaniesService : ICompaniesService
    {
        private readonly ICompaniesRepository _companiesRepository;

        public IList<AccountsCompaniesSet> GetCompanies()
        {
            return _companiesRepository.GetAll();
        }

        [ImportingConstructor]
        public CompaniesService(ICompaniesRepository companiesRepository)
        {
            _companiesRepository = companiesRepository;
        }
    }
}
