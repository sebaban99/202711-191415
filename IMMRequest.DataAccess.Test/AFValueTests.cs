using IMMRequest.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace IMMRequest.DataAccess.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AFValueTests
    {
        AFValue aFValue = new AFValue()
        {
            Request = new Request(),
            AdditionalField = new AdditionalField(),
            Value = "ABXE1234"
        };

        private AFValueRepository aFValueRepositoryInMemory = new AFValueRepository(
            ContextFactory.GetNewContext(ContextType.MEMORY));

        [TestInitialize]
        public void Initialize()
        {
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
