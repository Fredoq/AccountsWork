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
    public class TypesServiceTests
    {
        private Mock<ITypesRepository> _typesRepo;
        private TypesService _typesService;
        private readonly List<TypeSet> _data = new List<TypeSet>
            {
                new TypeSet
                {
                    Id=1,
                    Type = "Генподряд"
                    
                },
                new TypeSet
                {
                    Id=1,
                    Type = "Оборудование"
                }

            };

        [TestInitialize]
        public void SetUp()
        {
            _typesRepo = new Mock<ITypesRepository>();
            _typesService = new TypesService(_typesRepo.Object);
           

        }

        [TestMethod]
        public void GeTypesTest()
        {
            _typesRepo.Setup(x => x.GetAll()).
                Returns(_data);
            var types = _typesService.GetTypes();
            Assert.AreEqual(2, types.Count);
        }
    }

    
}
