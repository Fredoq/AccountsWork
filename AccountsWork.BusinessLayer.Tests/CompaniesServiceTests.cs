using System;
using System.Collections.Generic;
using System.Linq;
using AccountsWork.DataAccessLayer;
using AccountsWork.DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AccountsWork.BusinessLayer.Tests
{
    [TestClass]
    public class CompaniesServiceTests
    {
        private Mock<ICompaniesRepository> _companiesRepo;
        private CompaniesService _companiesService;
        private readonly List<AccountsCompaniesSet> _data = new List<AccountsCompaniesSet>
            {
                new AccountsCompaniesSet
                {
                    Id=1,
                    Company="ККС"
                },
                new AccountsCompaniesSet
                {
                    Id=2,
                    Company="ОПС"
                }

            };

        [TestInitialize]
        public void SetUp()
        {
            _companiesRepo = new Mock<ICompaniesRepository>();
            _companiesService = new CompaniesService(_companiesRepo.Object);
           

        }

        [TestMethod]
        public void GeCompaniesTest()
        {
            _companiesRepo.Setup(x => x.GetAll()).
                Returns(_data);
            var companies = _companiesService.GetCompanies();
            Assert.AreEqual(2, companies.Count);
        }
    }

    
}
