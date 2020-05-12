
using IMMRequest.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMMRequest.DataAccess.Tests
{
    [TestClass]
    public class AdminRepositoryTests
    {
        Admin sebaAdmin = new Admin()
        {
            Email = "seba@asd.com",
            Password = "Pass",
            Name = "Sebastian",
            Id = Guid.NewGuid()
        };

        Admin marcosAdmin = new Admin()
        {
            Email = "marcos@asd.com",
            Password = "Pass",
            Name = "marcos",
            Id = Guid.NewGuid()
        };

        private AdminRepository adminRepositoryInMemory = new AdminRepository(
            ContextFactory.GetNewContext(ContextType.MEMORY));

        [TestInitialize]
        public void Initialize()
        {
            adminRepositoryInMemory.Empty();
        }

        [TestMethod]
        public void GetAdmin_InexistentAdmin_ShouldReturnNull()
        {
            Admin adminById = adminRepositoryInMemory.Get(sebaAdmin.Id);
            Assert.IsNull(adminById);
        }

        [TestMethod]
        public void GetAdmin_ExistentAdmin_ShouldReturnSpecificAdminFromDB()
        {

            adminRepositoryInMemory.Add(sebaAdmin);
            adminRepositoryInMemory.Add(marcosAdmin);
            adminRepositoryInMemory.SaveChanges();

            Assert.AreEqual(adminRepositoryInMemory.Get(sebaAdmin.Id), sebaAdmin);
        }
    }
}
