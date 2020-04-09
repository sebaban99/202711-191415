using IMMRequest.DataAccess.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using IMMRequest.Domain;
namespace IMMRequest.DataAccess.Tests
{
    [TestClass]
    public class AreaRepositoryTests
    {
        Area cleaningArea = new Area() 
        {
            Id = Guid.NewGuid(),
            Name = "Limpieza",
            Topics = new List<Topic>()
        };

        Area transportArea = new Area()
        {
            Id = Guid.NewGuid(),
            Name = "Transporte",
            Topics = new List<Topic>()
        };

        private AreaRepository areaRepositoryInMemory = new AreaRepository(
            ContextFactory.GetNewContext(ContextType.MEMORY));

        [TestInitialize]
        public void Initialize()
        {
            areaRepositoryInMemory.Empty();
        }

        [TestMethod]
        public void GetArea_InexistentArea_ShouldReturnNull()
        {
            Area areaById = areaRepositoryInMemory.Get(cleaningArea.Id);
            Assert.AreEqual(null, areaById);
        }

        [TestMethod]
        public void GetArea_ExistentArea_ShouldReturnSpecificAreaFromDB()
        {
            areaRepositoryInMemory.Add(cleaningArea);
            areaRepositoryInMemory.Add(transportArea);

            Assert.AreEqual(areaRepositoryInMemory.Get(cleaningArea.Id),
                cleaningArea);
        }
    }
}
