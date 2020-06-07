using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.Exceptions;
using IMMRequest.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using AFRangeItem = IMMRequest.Domain.AFRangeItem;
using Type = IMMRequest.Domain.Type;
using System.Diagnostics.CodeAnalysis;

namespace IMMRequest.WebApi.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class RequestControllerTests
    {
        private static Topic topic = new Topic()
        {
            Id = Guid.NewGuid(),
            Types = new List<Type>(),
            Area = new Area(),
            Name = "Contenedores de basura"
        };

        private static Type type = new Type()
        {
            Id = Guid.NewGuid(),
            Name = "Contenedor roto",
            Topic = topic,
            AdditionalFields = new List<AdditionalField>()
        };

        static AdditionalField stateOfContainer = new AdditionalField()
        {
            Id = Guid.NewGuid(),
            FieldType = FieldType.Texto,
            Type = type,
            Name = "Estado del contenedor",
            Range = new List<AFRangeItem>()
        };

        AFRangeItem range = new AFRangeItem()
        {
            Id = Guid.NewGuid(),
            AdditionalField = stateOfContainer,
            Value = "Incendiado o chocado"
        };

        private static Request oneRequest = new Request()
        {
            Id = Guid.NewGuid(),
            RequestNumber = 1,
            Type = type,
            Details = "El contenedor de la esquina Av Gral Rivera" +
            " y Eduardo Mac Eachen fue incendiado en la madrugada del sábado 21 de marzo",
            Name = "Juan Perez",
            Email = "juan@perez.com.uy",
            Phone = "099 123 456",
            AddFieldValues = new List<AFValue>()
        };

        private static Request anotherRequest = new Request()
        {
            Id = Guid.NewGuid(),
            RequestNumber = 2,
            Type = type,
            Details = "El contenedor de la esquina Av Gral Rivera" +
            " y Eduardo Mac Eachen fue incendiado en la madrugada del sábado 21 de marzo",
            Name = "Juan Rodriguez",
            Email = "jro@gmail.com.uy",
            Phone = "099 123 123",
            AddFieldValues = new List<AFValue>()
        };

        AFValueItem item = new AFValueItem()
        {
            AFValue = oneStateOfContainerValue,
            Value = "Incendiado o chocado"
        };

        AFValueItem item2 = new AFValueItem()
        {
            AFValue = anotherStateOfContainerValue,
            Value = "Incendiado o chocado"
        };

        static AFValue anotherStateOfContainerValue = new AFValue()
        {
            Values = new List<AFValueItem>(),
            Request = anotherRequest,
            AdditionalField = stateOfContainer
        };

        static AFValue oneStateOfContainerValue = new AFValue()
        {
            Values = new List<AFValueItem>(),
            Request = oneRequest,
            AdditionalField = stateOfContainer
        };

        [TestInitialize]
        public void Initialize()
        {
            oneStateOfContainerValue.Values.Add(item);
            anotherStateOfContainerValue.Values.Add(item);
            stateOfContainer.Range.Add(range);
            type.AdditionalFields.Add(stateOfContainer);
            topic.Types.Add(type);
            oneRequest.AddFieldValues.Add(oneStateOfContainerValue);
            anotherRequest.AddFieldValues.Add(anotherStateOfContainerValue);

        }

        [TestMethod]
        public void GetAllRequestsCaseEmpty()
        {
            var requests = new List<Request>();
            var requestLogicMock = new Mock<IRequestLogic>(MockBehavior.Strict);

            requestLogicMock.Setup(m => m.GetAll()).Returns(requests);
            var requestController = new RequestController(requestLogicMock.Object);

            var result = requestController.Get();
            var okResult = result as OkObjectResult;
            var value = okResult.Value as List<RequestDTO>;

            requestLogicMock.VerifyAll();
            for (int i = 0; i < requests.Count; i++)
            {
                Assert.AreEqual(value[i], requests[i]);
            }
        }

        [TestMethod]
        public void GetAllRequestsCaseNotEmpty()
        {
            var requests = new List<Request>();
            requests.Add(oneRequest);
            requests.Add(anotherRequest);
            var requestLogicMock = new Mock<IRequestLogic>(MockBehavior.Strict);

            requestLogicMock.Setup(m => m.GetAll()).Returns(requests);
            var requestController = new RequestController(requestLogicMock.Object);
            var requestDTOs = new List<RequestDTO>();
            foreach (Request req in requests)
            {
                RequestDTO tm = new RequestDTO(req);
                requestDTOs.Add(tm);
            }

            var result = requestController.Get();
            var okResult = result as OkObjectResult;
            var value = okResult.Value as List<RequestDTO>;

            requestLogicMock.VerifyAll();
            for (int i = 0; i < requests.Count; i++)
            {
                Assert.AreEqual(value[i], requestDTOs[i]);
            }
        }

        [TestMethod]
        public void GetAllRequestsCaseErrorInDB()
        {
            var requests = new List<Request>();
            var requestLogicMock = new Mock<IRequestLogic>(MockBehavior.Strict);

            requestLogicMock.Setup(m => m.GetAll()).Throws(
                new DataAccessException("Error: could not get Table's elements"));
            var requestController = new RequestController(requestLogicMock.Object);

            var result = requestController.Get();
            var okResult = result as ObjectResult;
            var value = okResult.Value;

            requestLogicMock.VerifyAll();

            Assert.AreEqual(value, "Error: could not get Table's elements");
            Assert.AreEqual(okResult.StatusCode, 500);
        }

        [TestMethod]
        public void GetRequestByIdCaseExist()
        {
            var requestDTO = new RequestDTO(oneRequest);

            var requestLogicMock = new Mock<IRequestLogic>(MockBehavior.Strict);

            requestLogicMock.Setup(m => m.Get(oneRequest.Id)).Returns(oneRequest);
            var typeController = new RequestController(requestLogicMock.Object);

            var result = typeController.Get(oneRequest.Id);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as RequestDTO;

            requestLogicMock.VerifyAll();

            Assert.AreEqual(requestDTO, value);
        }

        [TestMethod]
        public void GetRequestByIdNotCaseExist()
        {
            var requestDTO = new RequestDTO(oneRequest);

            var requestLogicMock = new Mock<IRequestLogic>(MockBehavior.Strict);

            requestLogicMock.Setup(m => m.Get(oneRequest.Id)).Throws(
                new BusinessLogicException("Error: Invalid ID, Request does not exist"));
            var requestController = new RequestController(requestLogicMock.Object);

            var result = requestController.Get(oneRequest.Id);
            var okResult = result as ObjectResult;
            var value = okResult.Value;

            requestLogicMock.VerifyAll();
            Assert.AreEqual(value, "Error: Invalid ID, Request does not exist");
            Assert.AreEqual(okResult.StatusCode, 404);
        }

        [TestMethod]
        public void GetRequestCaseErrorInDB()
        {
            var requestDTO = new RequestDTO(oneRequest);

            var requestLogicMock = new Mock<IRequestLogic>(MockBehavior.Strict);

            requestLogicMock.Setup(m => m.Get(oneRequest.Id)).Throws(
                new DataAccessException("Error: could not retrieve Entity"));
            var requestController = new RequestController(requestLogicMock.Object);

            var result = requestController.Get(oneRequest.Id);
            var okResult = result as ObjectResult;
            var value = okResult.Value;

            requestLogicMock.VerifyAll();
            Assert.AreEqual(value, "Error: could not retrieve Entity");
            Assert.AreEqual(okResult.StatusCode, 500);
        }

        [TestMethod]
        public void GetRequestByRequestNumberCaseExist()
        {
            var requestDTO = new RequestDTO(oneRequest);

            var requestLogicMock = new Mock<IRequestLogic>(MockBehavior.Strict);
            requestLogicMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Request, bool>>>())).Returns(oneRequest);

            var typeController = new RequestController(requestLogicMock.Object);

            var result = typeController.Get(oneRequest.RequestNumber);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as RequestDTO;

            requestLogicMock.VerifyAll();

            Assert.AreEqual(requestDTO, value);
        }

        [TestMethod]
        public void GetRequestByRequestNumberCaseNotExist()
        {
            var requestDTO = new RequestDTO(oneRequest);

            var requestLogicMock = new Mock<IRequestLogic>(MockBehavior.Strict);

            requestLogicMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Request, bool>>>())).Throws(
            new BusinessLogicException("Error: could not retrieve the specific Request"));

            var requestController = new RequestController(requestLogicMock.Object);

            var result = requestController.Get(oneRequest.RequestNumber);
            var okResult = result as ObjectResult;
            var value = okResult.Value;

            requestLogicMock.VerifyAll();
            Assert.AreEqual(value, "Error: could not retrieve the specific Request");
            Assert.AreEqual(okResult.StatusCode, 404);
        }

        [TestMethod]
        public void GetRequestByRequestNumberCaseErrorInDB()
        {
            var requestDTO = new RequestDTO(oneRequest);

            var requestLogicMock = new Mock<IRequestLogic>(MockBehavior.Strict);

            requestLogicMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Request, bool>>>())).Throws(
            new DataAccessException("Error: could not retrieve Entity"));

            var requestController = new RequestController(requestLogicMock.Object);

            var result = requestController.Get(oneRequest.RequestNumber);
            var okResult = result as ObjectResult;
            var value = okResult.Value;

            requestLogicMock.VerifyAll();
            Assert.AreEqual(value, "Error: could not retrieve Entity");
            Assert.AreEqual(okResult.StatusCode, 500);
        }

        [TestMethod]
        public void PostCaseValidRequest()
        {
            var requestDTO = new RequestDTO(oneRequest);
            var requestLogicMock = new Mock<IRequestLogic>(MockBehavior.Strict);

            requestLogicMock.Setup(m => m.Create(It.IsAny<Request>())).Returns(oneRequest.RequestNumber);

            var reauestController = new RequestController(requestLogicMock.Object);

            var result = reauestController.Post(requestDTO);
            var okResult = result as OkObjectResult;
            var value = okResult.Value;

            requestLogicMock.VerifyAll();

            Assert.AreEqual(oneRequest.RequestNumber, value);
        }

        [TestMethod]
        public void PostCaseInvalidRequestTypeNotExists()
        {
            var requestDTO = new RequestDTO(oneRequest);
            var requestLogicMock = new Mock<IRequestLogic>(MockBehavior.Strict);

            requestLogicMock.Setup(m => m.Create(It.IsAny<Request>())).Throws(
                new BusinessLogicException("Error: Request's Type does not exist"));

            var reauestController = new RequestController(requestLogicMock.Object);

            var result = reauestController.Post(requestDTO);
            var okResult = result as ObjectResult;
            var value = okResult.Value;

            requestLogicMock.VerifyAll();

            Assert.AreEqual(value, "Error: Request's Type does not exist");
            Assert.AreEqual(okResult.StatusCode, 400);
        }

        [TestMethod]
        public void PostCaseInvalidRequestEmptyFields()
        {
            var requestDTO = new RequestDTO(oneRequest);
            var requestLogicMock = new Mock<IRequestLogic>(MockBehavior.Strict);

            requestLogicMock.Setup(m => m.Create(It.IsAny<Request>())).Throws(
                new BusinessLogicException("Error: Request had empty fields"));

            var reauestController = new RequestController(requestLogicMock.Object);

            var result = reauestController.Post(requestDTO);
            var okResult = result as ObjectResult;
            var value = okResult.Value;

            requestLogicMock.VerifyAll();

            Assert.AreEqual(value, "Error: Request had empty fields");
            Assert.AreEqual(okResult.StatusCode, 400);
        }

        [TestMethod]
        public void PostCaseInvalidErrorInDB()
        {
            var requestDTO = new RequestDTO(oneRequest);
            var requestLogicMock = new Mock<IRequestLogic>(MockBehavior.Strict);

            requestLogicMock.Setup(m => m.Create(It.IsAny<Request>())).Throws(
                new DataAccessException("Error: Could not add entity to DB"));

            var reauestController = new RequestController(requestLogicMock.Object);

            var result = reauestController.Post(requestDTO);
            var okResult = result as ObjectResult;
            var value = okResult.Value;

            requestLogicMock.VerifyAll();

            Assert.AreEqual(value, "Error: Could not add entity to DB");
            Assert.AreEqual(okResult.StatusCode, 500);
        }

        [TestMethod]
        public void PutCaseValidRequestExist()
        {
            Request updatedRequest = oneRequest;
            updatedRequest.Description = "In process of being reviewed";
            updatedRequest.Status = Status.Revision;
            var updReqDTO = new RequestDTO(updatedRequest);
            var originalReqDTO = new RequestDTO(oneRequest);
            var requestLogicMock = new Mock<IRequestLogic>(MockBehavior.Strict);

            requestLogicMock.Setup(m => m.Update(It.IsAny<Request>())).Returns(updatedRequest);

            var reauestController = new RequestController(requestLogicMock.Object);

            var result = reauestController.Put(oneRequest.Id, originalReqDTO);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as RequestDTO;

            requestLogicMock.VerifyAll();

            Assert.AreEqual(updReqDTO, value);
        }

        [TestMethod]
        public void PutCaseInvalidRequestNotExist()
        {
            var requestDTO = new RequestDTO(oneRequest);
            var requestLogicMock = new Mock<IRequestLogic>(MockBehavior.Strict);

            requestLogicMock.Setup(m => m.Update(It.IsAny<Request>())).Throws(
                new BusinessLogicException("Error: Invalid ID, Request does not exist"));

            var reauestController = new RequestController(requestLogicMock.Object);

            var result = reauestController.Put(oneRequest.Id, requestDTO);
            var okResult = result as ObjectResult;
            var value = okResult.Value;

            requestLogicMock.VerifyAll();

            Assert.AreEqual(value, "Error: Invalid ID, Request does not exist");
            Assert.AreEqual(okResult.StatusCode, 400);
        }

        [TestMethod]
        public void PutCaseInvalidErrorInDB()
        {
            var requestDTO = new RequestDTO(oneRequest);
            var requestLogicMock = new Mock<IRequestLogic>(MockBehavior.Strict);

            requestLogicMock.Setup(m => m.Update(It.IsAny<Request>())).Throws(
                new DataAccessException("Error: Could not update Entity in DB"));

            var reauestController = new RequestController(requestLogicMock.Object);

            var result = reauestController.Put(oneRequest.Id, requestDTO);
            var okResult = result as ObjectResult;
            var value = okResult.Value;

            requestLogicMock.VerifyAll();

            Assert.AreEqual(value, "Error: Could not update Entity in DB");
            Assert.AreEqual(okResult.StatusCode, 500);
        }
    }
}
