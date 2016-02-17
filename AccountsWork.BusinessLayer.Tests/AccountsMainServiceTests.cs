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
        private Mock<IAccountsStatusRepository> _accountsStatusRepo;
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
            _accountsStatusRepo = new Mock<IAccountsStatusRepository>();
            _accountService = new AccountsMainService(_accountsRepo.Object, _accountsStatusRepo.Object);            
        } 

        [TestMethod]
        public void Repository_AddAccount_CallsServiceAddAccountAndAddStatus()
        {
            var account = new AccountsMainSet
            {
                AccountAmount = 45423.34M,
                AccountCompany = "Company4",
                AccountDate = Convert.ToDateTime("2015-12-23"),
                AccountMcdType = "ООО",
                AccountNumber = "456",
                AccountType = "Сервис",
                AccountYear = 2015,
                Id = 67
            };
            var status = new AccountsStatusDetailsSet
            {
                AccountMainId = 67,
                AccountStatus = "В обработке",
                Id = 1
            };
            _accountService.AddAccount(account);
            _accountsRepo.Verify(a => a.Add(account), Times.Once());
            _accountsStatusRepo.Verify(a => a.Add(status), Times.Once());
        }
              
    }

    
}
