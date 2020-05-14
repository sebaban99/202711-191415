using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace IMMRequest.WebApi.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class LoginControllerTests
    {
        private LoginDTO user = new LoginDTO()
        {
            Email = "seba@asd.com",
            Password = "Pass"
        };

        private Admin sebaAdmin = new Admin()
        {
            Email = "seba@asd.com",
            Password = "Pass",
            Name = "Sebastian",
            Id = Guid.NewGuid()
        };

        [TestMethod]
        public void LoginTestCaseValidCredentials()
        {
            var sessionLogicMock = new Mock<ISessionLogic>(MockBehavior.Strict);
            var logLogicMock = new Mock<ILogLogic>(MockBehavior.Strict);
            var adminLogicMock = new Mock<IAdminLogic>(MockBehavior.Strict);

            sessionLogicMock.Setup(m => m.ValidateLogin(user.Email, user.Password)).Returns(sebaAdmin);
            logLogicMock.Setup(m => m.Add(It.IsAny<Log>()));
            adminLogicMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Admin, bool>>>())).Returns(sebaAdmin);
            var loginController = new LoginController(sessionLogicMock.Object, adminLogicMock.Object,
                 logLogicMock.Object);

            var result = loginController.Login(user);
            var okResult = result as ObjectResult;
            var value = okResult.Value as Admin;

            sessionLogicMock.VerifyAll();

            Assert.AreEqual(sebaAdmin, value);
        }

        [TestMethod]
        public void LoginTestCaseWrongCredentials()
        {
            var sessionLogicMock = new Mock<ISessionLogic>(MockBehavior.Strict);
            var logLogicMock = new Mock<ILogLogic>(MockBehavior.Strict);
            var LogicMock = new Mock<ISessionLogic>(MockBehavior.Strict);
            var adminLogicMock = new Mock<IAdminLogic>(MockBehavior.Strict);

            sessionLogicMock.Setup(m => m.ValidateLogin(user.Email, user.Password)).Returns((Admin)null);
            var loginController = new LoginController(sessionLogicMock.Object, adminLogicMock.Object,
                 logLogicMock.Object);

            var result = loginController.Login(user);
            var okResult = result as ObjectResult;
            var value = okResult.Value;

            sessionLogicMock.VerifyAll();

            Assert.AreEqual(value, "Login error: Incorrect email or password");
            Assert.AreEqual(okResult.StatusCode, 400);
        }
    }
}
