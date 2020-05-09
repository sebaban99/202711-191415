using IMMRequest.BusinessLogic;
using IMMRequest.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Range = IMMRequest.Domain.Range;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.WebApi.Tests
{
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
            Range = new List<Range>()
        };

        Range range = new Range()
        {
            Id = Guid.NewGuid(),
            AdditionalFieldId = stateOfContainer.Id,
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

        AFValue anotherStateOfContainerValue = new AFValue()
        {
            Value = "Incendiado o chocado",
            RequestId = anotherRequest.Id,
            AddFieldId = stateOfContainer.Id
        };

        AFValue oneStateOfContainerValue = new AFValue()
        {
            Value = "Incendiado o chocado",
            RequestId = oneRequest.Id,
            AddFieldId = stateOfContainer.Id
        };

        [TestInitialize]
        public void Initialize()
        {
            //stateOfContainer.Range.Add(range);
            //type.AdditionalFields.Add(stateOfContainer);
            //topic.Types.Add(type);
            //request.AddFieldValues.Add(stateOfContainerValue);
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
    }
}
