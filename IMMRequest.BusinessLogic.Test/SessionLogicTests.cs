using IMMRequest.DataAccess.Interfaces;
using IMMRequest.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text;

namespace IMMRequest.BusinessLogic.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SessionLogicTests
    {
        [TestMethod]
        public void ValidateLoginWrongCredentials()
        {
            var sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);
            var adminRepositoryMock = new Mock<IRepository<Admin>>(MockBehavior.Strict);
            adminRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Admin, bool>>>())).Returns((Admin)null);

            var sessionLogic = new SessionLogic(adminRepositoryMock.Object, sessionRepositoryMock.Object);
            var result = sessionLogic.ValidateLogin("seba@gmail.com","password");
            
            sessionRepositoryMock.VerifyAll();
            sessionRepositoryMock.VerifyAll();
            
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ValidateLoginValidCredentials()
        {
            Admin admin = new Admin()
            {
                Email = "seba@gmail.com",
                Password = "Pass",
                Name = "Sebastian Perez",
                Id = Guid.NewGuid()
            };

            var sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);
            var adminRepositoryMock = new Mock<IRepository<Admin>>(MockBehavior.Strict);
            adminRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Admin, bool>>>())).Returns(admin);
            sessionRepositoryMock.Setup(m => m.Add(It.IsAny<Session>()));
            sessionRepositoryMock.Setup(m => m.ValidateSession(It.IsAny<Guid>())).Returns(false);
            sessionRepositoryMock.Setup(m => m.SaveChanges());

            var sessionLogic = new SessionLogic(adminRepositoryMock.Object, sessionRepositoryMock.Object);
            var result = sessionLogic.ValidateLogin("seba@gmail.com", "Pass");

            sessionRepositoryMock.VerifyAll();
            sessionRepositoryMock.VerifyAll();

            Assert.AreEqual(admin, result);
        }
    }
}
