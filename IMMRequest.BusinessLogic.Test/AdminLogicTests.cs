using IMMRequest.DataAccess.Interfaces;
using IMMRequest.Domain;
using IMMRequest.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.BusinessLogic.Tests
{
    [ExcludeFromCodeCoverage]
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
            var reqRepoMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            mock.Setup(m => m.Add(admin));
            mock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Admin, bool>>>())).Returns((Admin)null);
            mock.Setup(m => m.SaveChanges());

            adminLogic = new AdminLogic(mock.Object, reqRepoMock.Object);
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
            var reqRepoMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            mock.Setup(m => m.Add(admin));
            mock.Setup(m => m.SaveChanges());

            adminLogic = new AdminLogic(mock.Object, reqRepoMock.Object);
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
            var reqRepoMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            mock.Setup(m => m.Add(admin));
            mock.Setup(m => m.SaveChanges());

            adminLogic = new AdminLogic(mock.Object, reqRepoMock.Object);
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
            var reqRepoMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            mock.Setup(m => m.Add(admin));
            mock.Setup(m => m.SaveChanges());

            adminLogic = new AdminLogic(mock.Object, reqRepoMock.Object);
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
            var reqRepoMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            mock.Setup(m => m.Add(admin));
            mock.Setup(m => m.SaveChanges());

            adminLogic = new AdminLogic(mock.Object, reqRepoMock.Object);
            var result = adminLogic.Create(admin);

            mock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Admin with same email already registered")]
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
            var reqRepoMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            mock.Setup(m => m.Add(admin));
            mock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Admin, bool>>>())).Returns(admin);
            mock.Setup(m => m.SaveChanges());

            adminLogic = new AdminLogic(mock.Object, reqRepoMock.Object);
            var result = adminLogic.Create(admin);

            mock.VerifyAll();
            Assert.AreEqual(result, admin);
        }

        [TestMethod]
        public void UpdateAdminCaseValidName()
        {
            admin = new Admin()
            {
                Email = "seba@gmail.com",
                Password = "Pass",
                Name = "Sebastian Perez",
                Id = Guid.NewGuid()
            };

            var mock = new Mock<IRepository<Admin>>(MockBehavior.Strict);
            var reqRepoMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            mock.Setup(m => m.Add(admin));
            mock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Admin, bool>>>())).Returns((Admin)null);
            mock.Setup(m => m.Get(
               It.IsAny<Guid>())).Returns(admin);
            mock.Setup(m => m.Update(admin));
            mock.Setup(m => m.SaveChanges());
            adminLogic = new AdminLogic(mock.Object, reqRepoMock.Object);
            var adminCreted = adminLogic.Create(admin);
            adminCreted.Name = "SebaP";

            var result = adminLogic.Update(adminCreted);

            mock.VerifyAll();
            Assert.AreEqual(result.Name, admin.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Admin had empty fields")]
        public void UpdateAdminCaseInvalidName()
        {
            admin = new Admin()
            {
                Email = "seba@gmail.com",
                Password = "Pass",
                Name = "Sebastian Perez",
                Id = Guid.NewGuid()
            };

            var mock = new Mock<IRepository<Admin>>(MockBehavior.Strict);
            var reqRepoMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            mock.Setup(m => m.Add(admin));
            mock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Admin, bool>>>())).Returns((Admin)null);
            mock.Setup(m => m.Get(
               It.IsAny<Guid>())).Returns(admin);
            mock.Setup(m => m.Update(admin));
            mock.Setup(m => m.SaveChanges());
            adminLogic = new AdminLogic(mock.Object, reqRepoMock.Object);
            var adminCreted = adminLogic.Create(admin);
            adminCreted.Name = "";

            var result = adminLogic.Update(adminCreted);

            mock.VerifyAll();
            Assert.AreEqual(result.Password, admin.Password);
        }

        [TestMethod]
        public void UpdateAdminCaseValidEmail()
        {
            admin = new Admin()
            {
                Email = "seba@gmail.com",
                Password = "Pass",
                Name = "Sebastian Perez",
                Id = Guid.NewGuid()
            };

            var mock = new Mock<IRepository<Admin>>(MockBehavior.Strict);
            var reqRepoMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            mock.Setup(m => m.Add(admin));
            mock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Admin, bool>>>())).Returns((Admin)null);
            mock.Setup(m => m.Get(
               It.IsAny<Guid>())).Returns(admin);
            mock.Setup(m => m.Update(admin));
            mock.Setup(m => m.SaveChanges());
            adminLogic = new AdminLogic(mock.Object, reqRepoMock.Object);
            var adminCreted = adminLogic.Create(admin);
            adminCreted.Email = "seba2@outlook.com";

            var result = adminLogic.Update(adminCreted);

            mock.VerifyAll();
            Assert.AreEqual(result.Email, admin.Email);
        }


        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Invalid email format")]
        public void UpdateAdminCaseInvalidEmail()
        {
            admin = new Admin()
            {
                Email = "seba@gmail.com",
                Password = "Pass",
                Name = "Sebastian Perez",
                Id = Guid.NewGuid()
            };

            var mock = new Mock<IRepository<Admin>>(MockBehavior.Strict);
            var reqRepoMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            mock.Setup(m => m.Add(admin));
            mock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Admin, bool>>>())).Returns((Admin)null);
            mock.Setup(m => m.Get(
               It.IsAny<Guid>())).Returns(admin);
            mock.Setup(m => m.Update(admin));
            mock.Setup(m => m.SaveChanges());
            adminLogic = new AdminLogic(mock.Object, reqRepoMock.Object);
            var adminCreted = adminLogic.Create(admin);
            adminCreted.Email = "seba2@";

            var result = adminLogic.Update(adminCreted);

            mock.VerifyAll();
            Assert.AreEqual(result.Password, admin.Password);
        }

        [TestMethod]
        public void UpdateAdminCaseValidPassword()
        {
            admin = new Admin()
            {
                Email = "seba@gmail.com",
                Password = "Pass",
                Name = "Sebastian Perez",
                Id = Guid.NewGuid()
            };

            var mock = new Mock<IRepository<Admin>>(MockBehavior.Strict);
            var reqRepoMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            mock.Setup(m => m.Add(admin));
            mock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Admin, bool>>>())).Returns((Admin)null);
            mock.Setup(m => m.Get(
               It.IsAny<Guid>())).Returns(admin);
            mock.Setup(m => m.Update(admin));
            mock.Setup(m => m.SaveChanges());
            adminLogic = new AdminLogic(mock.Object, reqRepoMock.Object);
            var adminCreted = adminLogic.Create(admin);
            adminCreted.Password = "Password";

            var result = adminLogic.Update(adminCreted);

            mock.VerifyAll();
            Assert.AreEqual(result.Password, admin.Password);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Admin had empty fields")]
        public void UpdateAdminCaseInvalidPassword()
        {
            admin = new Admin()
            {
                Email = "seba@gmail.com",
                Password = "Pass",
                Name = "Sebastian Perez",
                Id = Guid.NewGuid()
            };

            var mock = new Mock<IRepository<Admin>>(MockBehavior.Strict);
            var reqRepoMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            mock.Setup(m => m.Add(admin));
            mock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Admin, bool>>>())).Returns((Admin)null);
            mock.Setup(m => m.Get(
               It.IsAny<Guid>())).Returns(admin);
            mock.Setup(m => m.Update(admin));
            mock.Setup(m => m.SaveChanges());
            adminLogic = new AdminLogic(mock.Object, reqRepoMock.Object);
            var adminCreted = adminLogic.Create(admin);
            adminCreted.Password = "";

            var result = adminLogic.Update(adminCreted);

            mock.VerifyAll();
            Assert.AreEqual(result.Password, admin.Password);
        }

        [TestMethod]
        public void DeleteAdminCaseAdminExists()
        {
            admin = new Admin()
            {
                Email = "seba@gmail.com",
                Password = "Pass",
                Name = "Sebastian Perez",
                Id = Guid.NewGuid()
            };

            var mock = new Mock<IRepository<Admin>>(MockBehavior.Strict);
            var reqRepoMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            mock.Setup(m => m.Add(admin));
            mock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Admin, bool>>>())).Returns((Admin)null);
            mock.Setup(m => m.Get(
               It.IsAny<Guid>())).Returns(admin);
            mock.Setup(m => m.Remove(It.IsAny<Admin>()));
            mock.Setup(m => m.SaveChanges());

            adminLogic = new AdminLogic(mock.Object, reqRepoMock.Object);
            var adminCreted = adminLogic.Create(admin);

            adminLogic.Remove(adminCreted);

            mock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Admin to delete doesn't exist")]
        public void DeleteAdminCaseAdminNotExists()
        {
            admin = new Admin()
            {
                Email = "seba@gmail.com",
                Password = "Pass",
                Name = "Sebastian Perez",
                Id = Guid.NewGuid()
            };

            var mock = new Mock<IRepository<Admin>>(MockBehavior.Strict);
            var reqRepoMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            mock.Setup(m => m.Add(admin));
            mock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Admin, bool>>>())).Returns((Admin)null);
            mock.Setup(m => m.Get(
               It.IsAny<Guid>())).Returns((Admin)null);
            mock.Setup(m => m.SaveChanges());
            mock.Setup(m => m.Remove(It.IsAny<Admin>()));

            adminLogic = new AdminLogic(mock.Object, reqRepoMock.Object);

            adminLogic.Remove(admin);

            mock.VerifyAll();
        }

        [TestMethod]
        public void GenerateReportTypeA()
        {
            Request req1 = new Request()
            {
                RequestNumber = 1,
                CreationDate = DateTime.Now,
                Email = "seba@mail.com",
                Status = Status.Creada
            };
            Request req2 = new Request()
            {
                CreationDate = DateTime.Now.AddDays(1),
                Email = "seba@mail.com",
                RequestNumber = 2,
                Status = Status.Creada
            };
            Request req3 = new Request()
            {
                CreationDate = DateTime.Now.AddDays(-1),
                Email = "seba@mail.com",
                RequestNumber = 3,
                Status = Status.Aceptada
            };

            ReportTypeAElement repAElem1 = new ReportTypeAElement()
            {
                Amount = 2,
                Status = Status.Creada,
                RequestNumbers = new List<int>()
            };
            repAElem1.RequestNumbers.Add(req1.RequestNumber);
            repAElem1.RequestNumbers.Add(req2.RequestNumber);

            ReportTypeAElement repAElem2 = new ReportTypeAElement()
            {
                Amount = 1,
                Status = Status.Aceptada,
                RequestNumbers = new List<int>()
            };
            repAElem2.RequestNumbers.Add(req3.RequestNumber);

            List<Request> requests = new List<Request>();
            requests.Add(req1);
            requests.Add(req2);
            requests.Add(req3);

            List<ReportTypeAElement> report = new List<ReportTypeAElement>();
            report.Add(repAElem1);
            report.Add(repAElem2);

            var reqRepoMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var adminRepoMock = new Mock<IRepository<Admin>>(MockBehavior.Strict);
            reqRepoMock.Setup(m => m.GetAllByCondition(It.IsAny< Expression<Func<Request, bool>>>())).Returns(requests);
            
            var adminLogic = new AdminLogic(adminRepoMock.Object, reqRepoMock.Object);

            List<ReportTypeAElement> reportGenerated = (List<ReportTypeAElement>)adminLogic.GenerateReportA(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(+1), "seba@gmail.com");
            
            reqRepoMock.VerifyAll();

            Assert.IsTrue(report.SequenceEqual(reportGenerated));
        }

        [TestMethod]
        public void GenerateReportTypeB()
        {
            Area oneArea = new Area()
            {
                Name = "Limpieza"
            };

            Topic oneTopic = new Topic()
            {
                Name = "Limpieza Ciudad",
                Area = oneArea
            };

            Type oneType = new Type()
            {
                Name = "Liempieza Calle",
                Topic = oneTopic,
                CreationDate = DateTime.Now
            };

            Type anotherType = new Type()
            {
                Name = "Liempieza Cuadra",
                Topic = oneTopic,
                CreationDate = DateTime.Now.AddDays(-3)
            };

            Type anotherType2 = new Type()
            {
                Name = "Liempieza Barrio",
                Topic = oneTopic,
                CreationDate = DateTime.Now.AddDays(-2)
            };

            Request req1 = new Request()
            {
                RequestNumber = 1,
                CreationDate = DateTime.Now,
                Email = "seba@mail.com",
                Status = Status.Creada,
                Type = oneType
            };
            Request req2 = new Request()
            {
                Email = "seba@mail.com",
                RequestNumber = 2,
                Status = Status.Creada,
                Type = oneType
            };
            Request req3 = new Request()
            {
                Email = "seba@mail.com",
                RequestNumber = 3,
                Status = Status.Aceptada,
                Type = anotherType
            };

            Request req4 = new Request()
            {
                Email = "seba@mail.com",
                RequestNumber = 4,
                Status = Status.Aceptada,
                Type = anotherType2
            };

            ReportTypeBElement repBElem1 = new ReportTypeBElement()
            {
                Amount = 2,
                Type = oneType,
            };

            ReportTypeBElement repBElem2 = new ReportTypeBElement()
            {
                Amount = 1,
                Type = anotherType,
            };

            ReportTypeBElement repBElem3 = new ReportTypeBElement()
            {
                Amount = 1,
                Type = anotherType2,
            };

            List<Request> requests = new List<Request>();
            requests.Add(req1);
            requests.Add(req2);
            requests.Add(req3);
            requests.Add(req4);

            List<ReportTypeBElement> report = new List<ReportTypeBElement>();
            report.Add(repBElem1);
            report.Add(repBElem2);
            report.Add(repBElem3);

            var reqRepoMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var adminRepoMock = new Mock<IRepository<Admin>>(MockBehavior.Strict);
            reqRepoMock.Setup(m => m.GetAllByCondition(It.IsAny<Expression<Func<Request, bool>>>())).Returns(requests);

            var adminLogic = new AdminLogic(adminRepoMock.Object, reqRepoMock.Object);

            List<ReportTypeBElement> reportGenerated = (List<ReportTypeBElement>)adminLogic.GenerateReportB(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(+1));

            reqRepoMock.VerifyAll();

            Assert.IsTrue(report.SequenceEqual(reportGenerated));
        }
    }
}

