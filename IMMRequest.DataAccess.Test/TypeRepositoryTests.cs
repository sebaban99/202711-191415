﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMMRequest.DataAccess.Repositories;
using IMMRequest.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.DataAccess.Tests
{
    [TestClass]
    public class TypeRepositoryTests
    {
        Type brokenContainer = new Type()
        {
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
        public void GetType_ExistentType_ShouldReturnSpecificTypeFromDB()
        {

            typeRepositoryInMemory.Add(brokenContainer);
            typeRepositoryInMemory.Add(brokenLight);
            typeRepositoryInMemory.SaveChanges();

            Assert.AreEqual(typeRepositoryInMemory.Get(brokenContainer.Id), brokenContainer);
        }
    }
}