﻿using IMMRequest.DataAccess;
using IMMRequest.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace IMMRequest.BusinessLogic.Test
{
    [TestClass]
    public class AdminLogicTests
    {
        private Admin admin;
        private AdminLogic adminLogic;

        [TestMethod]
        public void CreateAdmin_()
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
            mock.Setup(m => m.GetByCondition(It.IsAny<Expression<Func<Admin, bool>>>())).Returns((Admin)null);
            mock.Setup(m => m.SaveChanges());

            adminLogic = new AdminLogic(mock.Object);
            var result = adminLogic.Create(admin);

            mock.VerifyAll();
            Assert.AreEqual(result, admin.Id);
        }

    }
}
