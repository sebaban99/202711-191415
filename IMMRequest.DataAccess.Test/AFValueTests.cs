using IMMRequest.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IMMRequest.DataAccess.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AFValueTests
    {
        AFValueItem item = new AFValueItem()
        {
            Value = "ABXE1234"
        };
        AFValue aFValue = new AFValue()
        {
            Request = new Request(),
            AdditionalField = new AdditionalField(),
            Values = new List<AFValueItem>()
        };

        private AFValueRepository aFValueRepositoryInMemory = new AFValueRepository(
            ContextFactory.GetNewContext(ContextType.MEMORY));

        [TestInitialize]
        public void Initialize()
        {
            aFValue.Values.Add(item);
            aFValueRepositoryInMemory.Empty();
        }

        [TestMethod]
        public void GetAFValue_Inexistent_ShouldReturnNull()
        {
            AFValue aFValueById = aFValueRepositoryInMemory.Get(aFValue.Id);
            Assert.IsNull(aFValueById);
        }

        [TestMethod]
        public void GetAFValue_Existent_ShouldReturnSpecificAFValueFromDB()
        {
            aFValueRepositoryInMemory.Add(aFValue);
            aFValueRepositoryInMemory.SaveChanges();

            Assert.AreEqual(aFValueRepositoryInMemory.Get(aFValue.Id),
                aFValue);
        }
    }
}
