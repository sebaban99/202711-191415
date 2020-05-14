using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMMRequest.Domain;
using System;
using System.Collections.Generic;
using Type = IMMRequest.Domain.Type;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace IMMRequest.DataAccess.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TypeRepositoryTests
    {
        Type brokenContainer = new Type()
        {
            IsActive = true,
            Topic = new Topic(),
            Name = "Contenedor roto",
            Id = Guid.NewGuid(),
            AdditionalFields = new List<AdditionalField>()
        };

        Type brokenLight = new Type()
        {
            Topic = new Topic(),
            Name = "Poste de luz roto",
            Id = Guid.NewGuid(),
            IsActive = true,
            AdditionalFields = new List<AdditionalField>()
        };

        private TypeRepository typeRepositoryInMemory = new TypeRepository(
            ContextFactory.GetNewContext(ContextType.MEMORY));

        [TestInitialize]
        public void Initialize()
        {
            typeRepositoryInMemory.Empty();
        }

        [TestMethod]
        public void GetType_InexistentType_ShouldReturnNull()
        {
            Type typeById = typeRepositoryInMemory.Get(brokenContainer.Id);
            Assert.IsNull(typeById);
        }

        [TestMethod]
        public void SoftDeleteType()
        {
            typeRepositoryInMemory.Add(brokenContainer);
            typeRepositoryInMemory.Add(brokenLight);
            typeRepositoryInMemory.SaveChanges();
            typeRepositoryInMemory.SoftDelete(brokenContainer);
            
            Assert.AreEqual(false, typeRepositoryInMemory.Get(brokenContainer.Id).IsActive);
        }

        [TestMethod]
        public void GetActiveTypes()
        {
            typeRepositoryInMemory.Add(brokenContainer);
            typeRepositoryInMemory.Add(brokenLight);
            typeRepositoryInMemory.SaveChanges();
            typeRepositoryInMemory.SoftDelete(brokenContainer);
            typeRepositoryInMemory.SaveChanges();

            List<Type> types = (List<Type>)typeRepositoryInMemory.GetActiveTypes().ToList();
            Assert.AreEqual(1, types.Count);
        }

        [TestMethod]
        public void GetType_ExistentType_ShouldReturnSpecificTypeFromDB()
        {
            typeRepositoryInMemory.Add(brokenContainer);
            typeRepositoryInMemory.Add(brokenLight);
            typeRepositoryInMemory.SaveChanges();

            Assert.AreEqual(typeRepositoryInMemory.Get(brokenContainer.Id), brokenContainer);
        }
    }
}
