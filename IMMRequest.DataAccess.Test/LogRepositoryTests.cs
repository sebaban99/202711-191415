using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System;
using IMMRequest.Domain;
using System.Collections.Generic;

namespace IMMRequest.DataAccess.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class LogRepositoryTests
    {
        
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
            Date = DateTime.Now,
            ActionType = "login"
        };

        private LogRepository logRepositoryInMemory = new LogRepository(
           ContextFactory.GetNewContext(ContextType.MEMORY));
        
        [TestInitialize()]
        public void Initialize()
        {
            logRepositoryInMemory.Empty();
        }

        [TestMethod]
        public void GetSessionExistentSessionShouldReturnNull()
        {
            Log logById = logRepositoryInMemory.Get(newLog.Id);
            Assert.IsNull(logById);
        }

        [TestMethod]
        public void GetSessionByDateSessionsExists()
        {
            newLog.Date = DateTime.Now.AddDays(-3);
            Log anotherLog = newLog;
            anotherLog.Date = DateTime.Now.AddDays(-1);
            logRepositoryInMemory.Add(newLog);
            logRepositoryInMemory.Add(anotherLog);
            logRepositoryInMemory.SaveChanges();

            List<Log> logsByDate = logRepositoryInMemory.GetLogsByDate(DateTime.Now.AddDays(-2), DateTime.Now);
            Assert.IsTrue(logsByDate.Count == 1);
        }

        [TestMethod]
        public void GetSessionByDateSessionsNotExists()
        {

            List<Log> logsByDate = logRepositoryInMemory.GetLogsByDate(DateTime.Now.AddDays(-2), DateTime.Now);
            Assert.IsTrue(logsByDate.Count == 0);
        }
    }
}
