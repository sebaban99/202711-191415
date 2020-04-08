using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using IMMRequest.Domain;
using System.Text;

namespace IMMRequest.DataAccess.Tests
{
    [TestClass] 
    public class AdditionalFieldRepositoryTests
    {
        AdditionalField stateOfContainer = new AdditionalField()
        {
            Id = Guid.NewGuid(),
            Name = "Estado del contenedor",
            Type = new Domain.Type(),
            Range = new List<Domain.Range>(),
            Value = "Conetendor roto"
        };

        AdditionalField stateOfStreetLight = new AdditionalField()
        {
            Id = Guid.NewGuid(),
            Name = "Estado del poste de luz",
            Type = new Domain.Type(),
            Range = new List<Domain.Range>(),
            Value = "Poste de luz roto"
        };

        private AdditionalFieldRepository addfieldRepositoryInMemory = new AdditionalFieldRepository(
            ContextFactory.GetNewContext(ContextType.MEMORY));

        [TestInitialize]
        public void Initialize()
        {
            addfieldRepositoryInMemory.Empty();
        }

        [TestMethod]
        public void GetAddField_InexistentAddFields_ShouldReturnNull()
        {
            AdditionalField addFieldById = addfieldRepositoryInMemory.Get(stateOfContainer.Id);
            Assert.AreEqual(null, addFieldById);
        }

        [TestMethod]
        public void GetType_ExistentType_ShouldReturnSpecificTypeFromDB()
        {

            addfieldRepositoryInMemory.Add(stateOfContainer);
            addfieldRepositoryInMemory.Add(stateOfStreetLight);

            Assert.AreEqual(addfieldRepositoryInMemory.Get(stateOfContainer.Id), stateOfContainer);
        }
    }
}
