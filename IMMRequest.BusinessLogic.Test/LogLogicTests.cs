using IMMRequest.DataAccess.Interfaces;
using IMMRequest.Domain;
using IMMRequest.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace IMMRequest.BusinessLogic.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class LogLogicTests
    {
        private LogLogic logLogic;
        private Log log;

        static Admin sebaAdmin = new Admin()
        {
            Email = "seba@asd.com",
            Password = "Pass",
            Name = "Sebastian",
            Id = Guid.NewGuid()
        };

        static Log newLog = new Log()
        {
            Id = Guid.NewGuid(),
            Admin = sebaAdmin,
            Email = sebaAdmin.Email,
            Date = DateTime.Now.AddDays(-3),
            ActionType = "login"
        };

        static Log anotherLog = new Log()
        {
            Id = Guid.NewGuid(),
            Admin = sebaAdmin,
            Email = sebaAdmin.Email,
            Date = DateTime.Now.AddDays(-1),
            ActionType = "login"
        };

        [TestMethod]
        public void GetAllLogs()
        {
            List<Log> logs = new List<Log>();

            var logRepositoryMock = new Mock<ILogRepository>(MockBehavior.Strict);
            logRepositoryMock.Setup(m => m.GetAll()).Returns(logs);

            logLogic = new LogLogic(logRepositoryMock.Object);
            var result = logLogic.GetAll();

            logRepositoryMock.VerifyAll();

            Assert.AreEqual(logs.Count, result.Count);
        }

        [TestMethod]
        public void GetLogsByDate()
        {
            List<Log> logs = new List<Log>();
            logs.Add(anotherLog);
            DateTime from = DateTime.Now.AddDays(-2);
            var logRepositoryMock = new Mock<ILogRepository>(MockBehavior.Strict);
            logRepositoryMock.Setup(m => m.GetLogsByDate(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(logs);

            logLogic = new LogLogic(logRepositoryMock.Object);
            var result = logLogic.GetLogsByDate(from, DateTime.Now);

            logRepositoryMock.VerifyAll();

            Assert.AreEqual(logs, result);
        }

        [TestMethod]
        public void AddLogNotNull()
        {
            var logRepositoryMock = new Mock<ILogRepository>(MockBehavior.Strict);
            logRepositoryMock.Setup(m => m.Add(It.IsAny<Log>()));
            logRepositoryMock.Setup(m => m.SaveChanges());

            logLogic = new LogLogic(logRepositoryMock.Object);
            logLogic.Add(anotherLog);

            logRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Log already exists")]
        public void AddNullLog()
        {
            var logRepositoryMock = new Mock<ILogRepository>(MockBehavior.Strict);
            logRepositoryMock.Setup(m => m.Add(It.IsAny<Log>()));
            logRepositoryMock.Setup(m => m.SaveChanges());

            logLogic = new LogLogic(logRepositoryMock.Object);
            logLogic.Add(null);

            logRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void RemoveLogNotNull()
        {
            var logRepositoryMock = new Mock<ILogRepository>(MockBehavior.Strict);
            logRepositoryMock.Setup(m => m.Remove(It.IsAny<Log>()));
            logRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(anotherLog);
            logRepositoryMock.Setup(m => m.SaveChanges());

            logLogic = new LogLogic(logRepositoryMock.Object);
            logLogic.Remove(anotherLog);

            logRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Log to remove was not found")]
        public void RemoveNullLog()
        {
            var logRepositoryMock = new Mock<ILogRepository>(MockBehavior.Strict);
            logRepositoryMock.Setup(m => m.Remove(It.IsAny<Log>()));
            logRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns((Log)null);
            logRepositoryMock.Setup(m => m.SaveChanges());

            logLogic = new LogLogic(logRepositoryMock.Object);
            logLogic.Remove(anotherLog);

            logRepositoryMock.VerifyAll();
        }
    }
}
