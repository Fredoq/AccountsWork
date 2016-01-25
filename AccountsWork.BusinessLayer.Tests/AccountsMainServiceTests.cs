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
    public class AccountsMainServiceTests
    {
        private Mock<IAccountsMainRepository> _accountsRepo;
        private AccountsMainService _accountService;
        private readonly List<AccountsMainSet> _data = new List<AccountsMainSet>
            {
                new AccountsMainSet
                {
                    AccountAmount = 100,
                    AccountCompany = "Company1",
                    AccountDate = Convert.ToDateTime("2015-04-13"),
                    AccountDescription = "None",
                    AccountMcdType = "ООО",
                    AccountNumber = "1",
                    AccountType = "Генподряд",
                    AccountYear = 2015,
                    Id = 1
                },
                new AccountsMainSet
                {
                    AccountAmount = 1462,
                    AccountCompany = "Company2",
                    AccountDate = Convert.ToDateTime("2015-07-23"),
                    AccountDescription = "WHAAT",
                    AccountMcdType = "ЗАО",
                    AccountNumber = "2",
                    AccountType = "Оборудование",
                    AccountYear = 2015,
                    Id = 1
                },
                new AccountsMainSet
                {
                    AccountAmount = 45423.34M,
                    AccountCompany = "Company3",
                    AccountDate = Convert.ToDateTime("2015-08-23"),
                    AccountMcdType = "ООО",
                    AccountNumber = "56",
                    AccountType = "Монтаж",
                    AccountYear = 2015,
                    Id = 5
                }
            };

        [TestInitialize]
        public void SetUp()
        {
            _accountsRepo = new Mock<IAccountsMainRepository>();
            _accountService = new AccountsMainService(_accountsRepo.Object);
           

        }

        [TestMethod]
        public void GeAccountsByYear()
        {
            _accountsRepo.Setup(x => x.GetAll()).
                Returns(_data);
            var accounts = _accountService.GetAccounts();
            Assert.AreEqual(3, accounts.Count);
        }
    }

    
}
