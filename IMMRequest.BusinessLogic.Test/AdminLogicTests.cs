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
            mock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Admin, bool>>>())).Returns((Admin)null);
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
            mock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Admin, bool>>>())).Returns(admin);
            mock.Setup(m => m.SaveChanges());

            adminLogic = new AdminLogic(mock.Object);
            var result = adminLogic.Create(admin);

            mock.VerifyAll();
            Assert.AreEqual(result, admin);
        }

        [TestMethod]
        public void UpdateAdminCaseValidNameUpdate()
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
            mock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Admin, bool>>>())).Returns((Admin)null);
            mock.Setup(m => m.Get(
               It.IsAny<Guid>())).Returns(admin);
            mock.Setup(m => m.Update(admin));
            mock.Setup(m => m.SaveChanges());
            adminLogic = new AdminLogic(mock.Object);
            var adminCreted = adminLogic.Create(admin);
            adminCreted.Name = "SebaP";

            var result = adminLogic.Update(adminCreted);

            mock.VerifyAll();
            Assert.AreEqual(result.Name, admin.Name);
        }

        [TestMethod]
        public void UpdateAdminCaseValidEmailUpdate()
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
            mock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Admin, bool>>>())).Returns((Admin)null);
            mock.Setup(m => m.Get(
               It.IsAny<Guid>())).Returns(admin);
            mock.Setup(m => m.Update(admin));
            mock.Setup(m => m.SaveChanges());
            adminLogic = new AdminLogic(mock.Object);
            var adminCreted = adminLogic.Create(admin);
            adminCreted.Email = "seba2@outlook.com";

            var result = adminLogic.Update(adminCreted);

            mock.VerifyAll();
            Assert.AreEqual(result.Email, admin.Email);
        }

        [TestMethod]
        public void UpdateAdminCaseValidPasswordUpdate()
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
            mock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Admin, bool>>>())).Returns((Admin)null);
            mock.Setup(m => m.Get(
               It.IsAny<Guid>())).Returns(admin);
            mock.Setup(m => m.Update(admin));
            mock.Setup(m => m.SaveChanges());
            adminLogic = new AdminLogic(mock.Object);
            var adminCreted = adminLogic.Create(admin);
            adminCreted.Password = "Password";

            var result = adminLogic.Update(adminCreted);

            mock.VerifyAll();
            Assert.AreEqual(result.Password, admin.Password);
        }
    }
}
