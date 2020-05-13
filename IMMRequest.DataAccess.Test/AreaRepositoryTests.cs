using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using IMMRequest.Domain;
using System.Diagnostics.CodeAnalysis;

namespace IMMRequest.DataAccess.Tests
{
    [ExcludeFromCodeCoverage]
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
            Assert.IsNull(areaById);
        }

        [TestMethod]
        public void GetArea_ExistentArea_ShouldReturnSpecificAreaFromDB()
        {
            areaRepositoryInMemory.Add(cleaningArea);
            areaRepositoryInMemory.Add(transportArea);
            areaRepositoryInMemory.SaveChanges();

            Assert.AreEqual(areaRepositoryInMemory.Get(cleaningArea.Id),
                cleaningArea);
        }
    }
}
