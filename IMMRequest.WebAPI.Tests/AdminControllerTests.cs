using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.Domain;
using IMMRequest.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace IMMRequest.WebApi.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AdminControllerTests
    {
        private Admin sebaAdmin = new Admin()
        {
            Email = "seba@asd.com",
            Password = "Pass",
            Name = "Sebastian",
            Id = Guid.NewGuid()
        };

        private Admin pedroAdmin = new Admin()
        {
            Email = "pedroa@asd.com",
            Password = "Password",
            Name = "Pedro",
            Id = Guid.NewGuid()
        };

        [TestMethod]
        public void GetAllAdminsCaseEmpty()
        {
            var admins = new List<Admin>();
            var adminLogicMock = new Mock<IAdminLogic>(MockBehavior.Strict);

            adminLogicMock.Setup(m => m.GetAll()).Returns(admins);
            var adminController = new AdminController(adminLogicMock.Object);

            var result = adminController.Get();
            var okResult = result as OkObjectResult;
            var value = okResult.Value as List<AdminDTO>;

            adminLogicMock.VerifyAll();
            for (int i = 0; i < admins.Count; i++)
            {
                Assert.AreEqual(value[i], admins[i]);
            }
        }

        [TestMethod]
        public void GetAllAdminsCaseNotEmpty()
        {
            var admins = new List<Admin>();
            admins.Add(sebaAdmin);
            admins.Add(pedroAdmin);

            var adminDTOs = new List<AdminDTO>();
            adminDTOs.Add(new AdminDTO(sebaAdmin));
            adminDTOs.Add(new AdminDTO(pedroAdmin));

            var adminLogicMock = new Mock<IAdminLogic>(MockBehavior.Strict);

            adminLogicMock.Setup(m => m.GetAll()).Returns(admins);
            var adminController = new AdminController(adminLogicMock.Object);

            var result = adminController.Get();
            var okResult = result as OkObjectResult;
            var value = okResult.Value as List<AdminDTO>;

            adminLogicMock.VerifyAll();
            for (int i = 0; i < adminDTOs.Count; i++)
            {
                Assert.AreEqual(value[i], adminDTOs[i]);
            }
        }

        [TestMethod]
        public void GetAdminByIdCaseExist()
        {
            var adminDTO = new AdminDTO(sebaAdmin);

            var adminLogicMock = new Mock<IAdminLogic>(MockBehavior.Strict);

            adminLogicMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(sebaAdmin);
            var adminController = new AdminController(adminLogicMock.Object);

            var result = adminController.Get(sebaAdmin.Id);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as AdminDTO;

            adminLogicMock.VerifyAll();

            Assert.AreEqual(adminDTO, value);
        }

        [TestMethod]
        public void GetAdminByIdCaseNotExist()
        {
            var adminDTO = new AdminDTO(sebaAdmin);

            var adminLogicMock = new Mock<IAdminLogic>(MockBehavior.Strict);

            adminLogicMock.Setup(m => m.Get(It.IsAny<Guid>())).Throws(
                new BusinessLogicException("Error: Invalid ID, Admin does not exist"));
            var adminController = new AdminController(adminLogicMock.Object);

            var result = adminController.Get(sebaAdmin.Id);
            var okResult = result as ObjectResult;
            var value = okResult.Value;

            adminLogicMock.VerifyAll();

            Assert.AreEqual(value, "Error: Invalid ID, Admin does not exist");
            Assert.AreEqual(okResult.StatusCode, 404);
        }

        [TestMethod]
        public void PostCaseValidAdmin()
        {
            var adminDTO = new AdminDTO(sebaAdmin);
            var adminLogicMock = new Mock<IAdminLogic>(MockBehavior.Strict);

            adminLogicMock.Setup(m => m.Create(It.IsAny<Admin>())).Returns(sebaAdmin);
            var adminController = new AdminController(adminLogicMock.Object);

            var result = adminController.Post(adminDTO);
            var okResult = result as OkObjectResult;
            var value = okResult.Value;

            adminLogicMock.VerifyAll();

            Assert.AreEqual(adminDTO, value);
        }

        [TestMethod]
        public void PostCaseInvalidAdminAlreadyRegistered()
        {
            var adminDTO = new AdminDTO(sebaAdmin);
            var adminLogicMock = new Mock<IAdminLogic>(MockBehavior.Strict);

            adminLogicMock.Setup(m => m.Create(It.IsAny<Admin>())).Throws(
                new BusinessLogicException("Error: Admin with same email already registered"));
            var adminController = new AdminController(adminLogicMock.Object);

            var result = adminController.Post(adminDTO);
            var okResult = result as ObjectResult;
            var value = okResult.Value;

            adminLogicMock.VerifyAll();

            Assert.AreEqual(value, "Error: Admin with same email already registered");
            Assert.AreEqual(okResult.StatusCode, 400);
        }

        [TestMethod]
        public void PostCaseInvalidAdminEmptyFields()
        {
            var adminDTO = new AdminDTO(sebaAdmin);
            var adminLogicMock = new Mock<IAdminLogic>(MockBehavior.Strict);

            adminLogicMock.Setup(m => m.Create(It.IsAny<Admin>())).Throws(
                new BusinessLogicException("Error: Admin had empty fields"));
            var adminController = new AdminController(adminLogicMock.Object);

            var result = adminController.Post(adminDTO);
            var okResult = result as ObjectResult;
            var value = okResult.Value;

            adminLogicMock.VerifyAll();

            Assert.AreEqual(value, "Error: Admin had empty fields");
            Assert.AreEqual(okResult.StatusCode, 400);
        }

        [TestMethod]
        public void DeleteCaseExistsType()
        {
            var adminLogicMock = new Mock<IAdminLogic>(MockBehavior.Strict);

            adminLogicMock.Setup(m => m.Remove(sebaAdmin));
            adminLogicMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(sebaAdmin);
            var adminController = new AdminController(adminLogicMock.Object);

            var result = adminController.Delete(sebaAdmin.Id);
            var okResult = result as OkObjectResult;

            adminLogicMock.VerifyAll();

            Assert.AreEqual(okResult.StatusCode, 200);
        }

        [TestMethod]
        public void DeleteCaseNotExistsAdmin()
        {
            var adminLogicMock = new Mock<IAdminLogic>(MockBehavior.Strict);

            adminLogicMock.Setup(m => m.Remove(sebaAdmin)).Throws(
                new BusinessLogicException("Error: Admin to delete doesn't exist"));
            adminLogicMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(sebaAdmin);
            var adminController = new AdminController(adminLogicMock.Object);

            var result = adminController.Delete(sebaAdmin.Id);
            var okResult = result as ObjectResult;
            var value = okResult.Value;

            adminLogicMock.VerifyAll();

            Assert.AreEqual(value, "Error: Admin to delete doesn't exist");
            Assert.AreEqual(okResult.StatusCode, 404);
        }

        [TestMethod]
        public void PutCaseValidAdminExist()
        {
            Admin updatedAdmin = sebaAdmin;
            updatedAdmin.Name = "Seba Ban";
            var updatedAdminDTO = new AdminDTO(updatedAdmin);
            var originalAdminDTO = new AdminDTO(sebaAdmin);
            var adminLogicMock = new Mock<IAdminLogic>(MockBehavior.Strict);

            adminLogicMock.Setup(m => m.Update(It.IsAny<Admin>())).Returns(updatedAdmin);

            var adminController = new AdminController(adminLogicMock.Object);

            var result = adminController.Put(sebaAdmin.Id, originalAdminDTO);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as AdminDTO;

            adminLogicMock.VerifyAll();

            Assert.AreEqual(updatedAdminDTO, value);
        }

        [TestMethod]
        public void PutCaseInvalidAdminNotExist()
        {
            var adminDTO = new AdminDTO(sebaAdmin);
            var adminLogicMock = new Mock<IAdminLogic>(MockBehavior.Strict);

            adminLogicMock.Setup(m => m.Update(It.IsAny<Admin>())).Throws(
                new BusinessLogicException("Error: Admin to update doesn't exist"));

            var adminController = new AdminController(adminLogicMock.Object);

            var result = adminController.Put(sebaAdmin.Id, adminDTO);
            var okResult = result as ObjectResult;
            var value = okResult.Value;

            adminLogicMock.VerifyAll();

            Assert.AreEqual(value, "Error: Admin to update doesn't exist");
            Assert.AreEqual(okResult.StatusCode, 400);
        }

        [TestMethod]
        public void PutCaseInvalidErrorInDB()
        {
            var adminDTO = new AdminDTO(sebaAdmin);
            var adminLogicMock = new Mock<IAdminLogic>(MockBehavior.Strict);

            adminLogicMock.Setup(m => m.Update(It.IsAny<Admin>())).Throws(
                new DataAccessException("Error: Could not update Entity in DB"));

            var adminController = new AdminController(adminLogicMock.Object);

            var result = adminController.Put(sebaAdmin.Id, adminDTO);
            var okResult = result as ObjectResult;
            var value = okResult.Value;

            adminLogicMock.VerifyAll();

            Assert.AreEqual(value, "Error: Could not update Entity in DB");
            Assert.AreEqual(okResult.StatusCode, 500);
        }
    }
}


