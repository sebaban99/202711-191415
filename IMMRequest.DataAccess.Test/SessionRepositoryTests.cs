using IMMRequest.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace IMMRequest.DataAccess.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SessionRepositoryTests
    {
        Admin sebaAdmin = new Admin()
        {
            Email = "seba@asd.com",
            Password = "Pass",
            Name = "Sebastian",
            Id = Guid.NewGuid()
        };

        Session session = new Session()
        {
            Id = Guid.NewGuid(),
            AdminId = Guid.NewGuid(),
        };
        

        private SessionRepository sessionRepositoryInMemory = new SessionRepository(
           ContextFactory.GetNewContext(ContextType.MEMORY));
        [TestInitialize]
        public void Initialize()
        {
            sessionRepositoryInMemory.Empty();
        }

        [TestMethod]
        public void GetSessionExistentSessionShouldReturnNull()
        {
            Session sessionById = sessionRepositoryInMemory.Get(session.Id);
            Assert.IsNull(sessionById);
        }

        [TestMethod]
        public void GetSessionExistentSessionShouldReturnSpecificTypeFromDB()
        {

            sessionRepositoryInMemory.Add(session);
            sessionRepositoryInMemory.SaveChanges();

            Assert.AreEqual(sessionRepositoryInMemory.Get(session.Id), session);
        }
    }
}
