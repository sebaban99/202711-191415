using IMMRequest.DataAccess;
using IMMRequest.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace IMMRequest.BusinessLogic.Tests
{
    [TestClass]
    public class AdminLogicTests
    {
        private Admin admin;
        private AdminLogic adminLogic;

        [TestMethod]
        public void CreateAdminCaseValidAdminNotExistsInDB()
        {
            admin = new Admin()
            {
                Email = "seba@gmail.com",
                Password = "Pass",
                Name = "Sebastian Perez",
                Id = Guid.NewGuid()
            };

            var mock = new Mock<IRepository<Admin>>(MockBehavior.Strict);
            mock.Setup(m => m.Add(admin));
            mock.Setup(m => m.SaveChanges());

            adminLogic = new AdminLogic(mock.Object);
            var result = adminLogic.Create(admin);

            mock.VerifyAll();
            Assert.AreEqual(result, admin);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Admin had empty fields")]
        public void CreateAdminCaseInvalidAdminEmptyName()
        {
            admin = new Admin()
            {
                Email = "seba@gmail.com",
                Password = "Pass",
                Name = "",
                Id = Guid.NewGuid()
            };

            var mock = new Mock<IRepository<Admin>>(MockBehavior.Strict);
            mock.Setup(m => m.Add(admin));
            mock.Setup(m => m.SaveChanges());

            adminLogic = new AdminLogic(mock.Object);
            var result = adminLogic.Create(admin);

            mock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Admin had empty fields")]
        public void CreateAdminCaseInvalidAdminEmptyEmail()
        {
            admin = new Admin()
            {
                Email = "",
                Password = "Pass",
                Name = "Sebastian Perez",
                Id = Guid.NewGuid()
            };

            var mock = new Mock<IRepository<Admin>>(MockBehavior.Strict);
            mock.Setup(m => m.Add(admin));
            mock.Setup(m => m.SaveChanges());

            adminLogic = new AdminLogic(mock.Object);
            var result = adminLogic.Create(admin);

            mock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Admin had empty fields")]
        public void CreateAdminCaseInvalidAdminEmptyPassword()
        {
            admin = new Admin()
            {
                Email = "seba@gmail.com",
                Password = "",
                Name = "Sebastian Perez",
                Id = Guid.NewGuid()
            };

            var mock = new Mock<IRepository<Admin>>(MockBehavior.Strict);
            mock.Setup(m => m.Add(admin));
            mock.Setup(m => m.SaveChanges());

            adminLogic = new AdminLogic(mock.Object);
            var result = adminLogic.Create(admin);

            mock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Invalid email format")]
        public void CreateAdminCaseInvalidAdminInvalidEmail()
        {
            admin = new Admin()
            {
                Email = "sebagmail.com",
                Password = "Pass",
                Name = "Sebastian Perez",
                Id = Guid.NewGuid()
            };

            var mock = new Mock<IRepository<Admin>>(MockBehavior.Strict);
            mock.Setup(m => m.Add(admin));
            mock.Setup(m => m.SaveChanges());

            adminLogic = new AdminLogic(mock.Object);
            var result = adminLogic.Create(admin);

            mock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Invalid email format")]
        public void CreateAdminCaseValidAdminCaseExistsInDB()
        {
            admin = new Admin()
            {
                Email = "seba@gmail.com",
                Password = "Pass",
                Name = "Sebastian Perez",
                Id = Guid.NewGuid()
            };

            var mock = new Mock<IRepository<Admin>>(MockBehavior.Strict);
            mock.Setup(m => m.Add(admin));
            mock.Setup(m => m.SaveChanges());

            adminLogic = new AdminLogic(mock.Object);
            var result = adminLogic.Create(admin);

            mock.VerifyAll();
            Assert.AreEqual(result, admin);
        }

    }
}
