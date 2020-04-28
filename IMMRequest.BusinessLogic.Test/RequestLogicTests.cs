using System;
using System.Collections.Generic;
using System.Text;
using Type = IMMRequest.Domain.Type;
using Range = IMMRequest.Domain.Range;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMMRequest.Domain;
using Moq;
using IMMRequest.DataAccess.Interfaces;

namespace IMMRequest.BusinessLogic.Tests
{
    [TestClass]
    public class RequestLogicTests
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

        static AdditionalField dateOfDiscovery = new AdditionalField()
        {
            Id = Guid.NewGuid(),
            FieldType = FieldType.Fecha,
            Type = type,
            Name = "Fecha de descubrimiento",
            Range = new List<Range>()
        };

        private static Request request = new Request()
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

        AFValue stateOfContainerValue = new AFValue()
        {
            Value = "Incendiado o chocado",
            RequestId = request.Id,
            AddFieldId = stateOfContainer.Id
        };

        AFValue dateOfDiscoveryValue = new AFValue()
        {
            Value = DateTime.Today.ToShortDateString(),
            RequestId = request.Id,
            AddFieldId = dateOfDiscovery.Id
        };

        List<Request> requestsInDB = new List<Request>();
        private RequestLogic requestLogic;

        [TestInitialize]
        public void Initialize()
        {
            stateOfContainer.Range.Add(range);
            type.AdditionalFields.Add(stateOfContainer);
            type.AdditionalFields.Add(dateOfDiscovery);
            topic.Types.Add(type);
            request.AddFieldValues.Add(stateOfContainerValue);
            request.AddFieldValues.Add(dateOfDiscoveryValue);
        }

        [TestCleanup]
        public void CleanUp()
        {
            requestsInDB.Clear();
            request.Type = type;
            foreach (AdditionalField af in request.Type.AdditionalFields)
            {
                af.Range.Clear();
            }
            request.Type.AdditionalFields.Clear();
            topic.Types.Clear();
            request.AddFieldValues.Clear();
            request.Details = "El contenedor de la esquina Av Gral Rivera" +
                " y Eduardo Mac Eachen fue incendiado en la madrugada del sábado 21 de marzo";
            request.Name = "Juan Perez";
            request.Email = "juan@perez.com.uy";
            request.Phone = "099 123 456";
        }

        [TestMethod]
        public void CreateRequestCaseValidRequestWithAddFields()
        {
            var requestRepositoryMock = new Mock<IRepository<Request>>(MockBehavior.Strict);
            var aFValueRepositoryMock = new Mock<IRepository<AFValue>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            requestRepositoryMock.Setup(m => m.GetAll()).Returns(requestsInDB);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);
            aFValueRepositoryMock.Setup(m => m.Add(It.IsAny<AFValue>()));
            aFValueRepositoryMock.Setup(m => m.SaveChanges());

            requestLogic = new RequestLogic(requestRepositoryMock.Object,
                aFValueRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            aFValueRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();

            Assert.AreEqual(result, request.RequestNumber);
        }

        [TestMethod]
        public void CreateRequestCaseValidRequestWithoutAddFields()
        {
            Topic trashContainers = new Topic()
            {
                Id = Guid.NewGuid(),
                Types = new List<Type>(),
                Area = new Area(),
                Name = "Contenedores de basura"
            };

            Type brokenContainer = new Type()
            {
                Id = Guid.NewGuid(),
                Name = "Contenedor roto",
                Topic = trashContainers,
                AdditionalFields = new List<AdditionalField>()
            };

            trashContainers.Types.Add(brokenContainer);

            Request aRequest = new Request()
            {
                Id = Guid.NewGuid(),
                RequestNumber = 1,
                Type = brokenContainer,
                Details = "El contenedor de",
                Name = "Juan Perez",
                Email = "juan@perez.com.uy",
                Phone = "099 123 456",
                AddFieldValues = new List<AFValue>()
            };

            var requestRepositoryMock = new Mock<IRepository<Request>>(MockBehavior.Strict);
            var aFValueRepositoryMock = new Mock<IRepository<AFValue>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(aRequest));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            requestRepositoryMock.Setup(m => m.GetAll()).Returns(requestsInDB);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(brokenContainer);

            requestLogic = new RequestLogic(requestRepositoryMock.Object,
                aFValueRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(aRequest);

            requestRepositoryMock.VerifyAll();
            aFValueRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();

            Assert.AreEqual(result, aRequest.RequestNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Request had empty fields")]
        public void CreateRequestCaseInvalidRequestEmptyName()
        {
            request.Name = "";

            var requestRepositoryMock = new Mock<IRepository<Request>>(MockBehavior.Strict);
            var aFValueRepositoryMock = new Mock<IRepository<AFValue>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            requestRepositoryMock.Setup(m => m.GetAll()).Returns(requestsInDB);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);
            aFValueRepositoryMock.Setup(m => m.Add(It.IsAny<AFValue>()));
            aFValueRepositoryMock.Setup(m => m.SaveChanges());

            requestLogic = new RequestLogic(requestRepositoryMock.Object,
                aFValueRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            aFValueRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Request had empty fields")]
        public void CreateRequestCaseInvalidRequestEmptyDetail()
        {
            request.Details = "";

            var requestRepositoryMock = new Mock<IRepository<Request>>(MockBehavior.Strict);
            var aFValueRepositoryMock = new Mock<IRepository<AFValue>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            requestRepositoryMock.Setup(m => m.GetAll()).Returns(requestsInDB);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);
            aFValueRepositoryMock.Setup(m => m.Add(It.IsAny<AFValue>()));
            aFValueRepositoryMock.Setup(m => m.SaveChanges());

            requestLogic = new RequestLogic(requestRepositoryMock.Object,
                aFValueRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            aFValueRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Request's Type does not exist")]
        public void CreateRequestCaseInvalidRequestNotExistsType()
        {
            var requestRepositoryMock = new Mock<IRepository<Request>>(MockBehavior.Strict);
            var aFValueRepositoryMock = new Mock<IRepository<AFValue>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            requestRepositoryMock.Setup(m => m.GetAll()).Returns(requestsInDB);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns((Type)null);
            aFValueRepositoryMock.Setup(m => m.Add(It.IsAny<AFValue>()));
            aFValueRepositoryMock.Setup(m => m.SaveChanges());

            requestLogic = new RequestLogic(requestRepositoryMock.Object,
                aFValueRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            aFValueRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Request had empty fields")]
        public void CreateRequestCaseInvalidRequestEmptyMail()
        {
            request.Email = "";

            var requestRepositoryMock = new Mock<IRepository<Request>>(MockBehavior.Strict);
            var aFValueRepositoryMock = new Mock<IRepository<AFValue>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            requestRepositoryMock.Setup(m => m.GetAll()).Returns(requestsInDB);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);
            aFValueRepositoryMock.Setup(m => m.Add(It.IsAny<AFValue>()));
            aFValueRepositoryMock.Setup(m => m.SaveChanges());

            requestLogic = new RequestLogic(requestRepositoryMock.Object,
                aFValueRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            aFValueRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Request had empty fields")]
        public void CreateRequestCaseInvalidRequestEmptyType()
        {
            request.Type = null;
            request.AddFieldValues.Clear();

            var requestRepositoryMock = new Mock<IRepository<Request>>(MockBehavior.Strict);
            var aFValueRepositoryMock = new Mock<IRepository<AFValue>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            requestRepositoryMock.Setup(m => m.GetAll()).Returns(requestsInDB);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns((Type)null);
            aFValueRepositoryMock.Setup(m => m.Add(It.IsAny<AFValue>()));
            aFValueRepositoryMock.Setup(m => m.SaveChanges());

            requestLogic = new RequestLogic(requestRepositoryMock.Object,
                aFValueRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            aFValueRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: One Request's additional field value was empty")]
        public void CreateRequestCaseInvalidRequestNullAFV()
        {
            request.AddFieldValues[0] = null;

            var requestRepositoryMock = new Mock<IRepository<Request>>(MockBehavior.Strict);
            var aFValueRepositoryMock = new Mock<IRepository<AFValue>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            requestRepositoryMock.Setup(m => m.GetAll()).Returns(requestsInDB);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);
            aFValueRepositoryMock.Setup(m => m.Add(It.IsAny<AFValue>()));
            aFValueRepositoryMock.Setup(m => m.SaveChanges());

            requestLogic = new RequestLogic(requestRepositoryMock.Object,
                aFValueRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            aFValueRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: One Request's additional field was not from type")]
        public void CreateRequestCaseInvalidRequestAFVNotFromType()
        {
            request.AddFieldValues[0].AddFieldId = Guid.NewGuid();

            var requestRepositoryMock = new Mock<IRepository<Request>>(MockBehavior.Strict);
            var aFValueRepositoryMock = new Mock<IRepository<AFValue>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            requestRepositoryMock.Setup(m => m.GetAll()).Returns(requestsInDB);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);
            aFValueRepositoryMock.Setup(m => m.Add(It.IsAny<AFValue>()));
            aFValueRepositoryMock.Setup(m => m.SaveChanges());

            requestLogic = new RequestLogic(requestRepositoryMock.Object,
                aFValueRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            aFValueRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: One Request's additional field value was empty")]
        public void CreateRequestCaseInvalidRequestAFVNUllValue()
        {
            request.AddFieldValues[0].Value = null;

            var requestRepositoryMock = new Mock<IRepository<Request>>(MockBehavior.Strict);
            var aFValueRepositoryMock = new Mock<IRepository<AFValue>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            requestRepositoryMock.Setup(m => m.GetAll()).Returns(requestsInDB);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);
            aFValueRepositoryMock.Setup(m => m.Add(It.IsAny<AFValue>()));
            aFValueRepositoryMock.Setup(m => m.SaveChanges());

            requestLogic = new RequestLogic(requestRepositoryMock.Object,
                aFValueRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            aFValueRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: One Request's additional field value was invalid, check data type")]
        public void CreateRequestCaseInvalidRequestAFVWrongValueDataTypeScenario1()
        {
            request.AddFieldValues[1].Value = "3 de mayo";

            var requestRepositoryMock = new Mock<IRepository<Request>>(MockBehavior.Strict);
            var aFValueRepositoryMock = new Mock<IRepository<AFValue>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            requestRepositoryMock.Setup(m => m.GetAll()).Returns(requestsInDB);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);
            aFValueRepositoryMock.Setup(m => m.Add(It.IsAny<AFValue>()));
            aFValueRepositoryMock.Setup(m => m.SaveChanges());

            requestLogic = new RequestLogic(requestRepositoryMock.Object,
                aFValueRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            aFValueRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: One Request's additional field value was invalid, check data type")]
        public void CreateRequestCaseInvalidRequestAFVWrongValueDataTypeScenario2()
        {
            request.AddFieldValues[0].Value = "14";

            var requestRepositoryMock = new Mock<IRepository<Request>>(MockBehavior.Strict);
            var aFValueRepositoryMock = new Mock<IRepository<AFValue>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            requestRepositoryMock.Setup(m => m.GetAll()).Returns(requestsInDB);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);
            aFValueRepositoryMock.Setup(m => m.Add(It.IsAny<AFValue>()));
            aFValueRepositoryMock.Setup(m => m.SaveChanges());

            requestLogic = new RequestLogic(requestRepositoryMock.Object,
                aFValueRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            aFValueRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: One Request's additional field value was invalid, check fields's range")]
        public void CreateRequestCaseInvalidRequestAFVNotInValidRangeScenario1()
        {
            Range yesterday = new Range()
            {
                Id = Guid.NewGuid(),
                Value = DateTime.Today.AddDays(-1).ToShortDateString(),
                AdditionalFieldId = dateOfDiscovery.Id
            };

            Range tomorrow = new Range()
            {
                Id = Guid.NewGuid(),
                Value = DateTime.Today.AddDays(1).ToShortDateString(),
                AdditionalFieldId = dateOfDiscovery.Id
            };

            dateOfDiscovery.Range.Add(yesterday);
            dateOfDiscovery.Range.Add(tomorrow);

            request.AddFieldValues[1].Value = DateTime.Today.AddDays(2).ToShortDateString();

            var requestRepositoryMock = new Mock<IRepository<Request>>(MockBehavior.Strict);
            var aFValueRepositoryMock = new Mock<IRepository<AFValue>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            requestRepositoryMock.Setup(m => m.GetAll()).Returns(requestsInDB);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);
            aFValueRepositoryMock.Setup(m => m.Add(It.IsAny<AFValue>()));
            aFValueRepositoryMock.Setup(m => m.SaveChanges());

            requestLogic = new RequestLogic(requestRepositoryMock.Object,
                aFValueRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            aFValueRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: One Request's additional field value was invalid, check fields's range")]
        public void CreateRequestCaseInvalidRequestAFVNotInValidRangeScenario2()
        {
            request.AddFieldValues[0].Value = "robado";

            var requestRepositoryMock = new Mock<IRepository<Request>>(MockBehavior.Strict);
            var aFValueRepositoryMock = new Mock<IRepository<AFValue>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            requestRepositoryMock.Setup(m => m.GetAll()).Returns(requestsInDB);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);
            aFValueRepositoryMock.Setup(m => m.Add(It.IsAny<AFValue>()));
            aFValueRepositoryMock.Setup(m => m.SaveChanges());

            requestLogic = new RequestLogic(requestRepositoryMock.Object,
                aFValueRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            aFValueRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: One Request's additional field value was invalid, check fields's range")]
        public void CreateRequestCaseInvalidRequestAFVNotInValidRangeScenario3()
        {
            AdditionalField amountOfContainersAffected = new AdditionalField()
            {
                Id = Guid.NewGuid(),
                FieldType = FieldType.Entero,
                Type = type,
                Name = "Cantidad de contenedores afectados en la cuadra",
                Range = new List<Range>()
            };

            Range minValue = new Range()
            {
                Id = Guid.NewGuid(),
                AdditionalFieldId = amountOfContainersAffected.Id,
                Value = "1"
            };

            Range maxValue = new Range()
            {
                Id = Guid.NewGuid(),
                AdditionalFieldId = amountOfContainersAffected.Id,
                Value = "5"
            };

            amountOfContainersAffected.Range.Add(minValue);
            amountOfContainersAffected.Range.Add(maxValue);
            request.Type.AdditionalFields.Add(amountOfContainersAffected);

            AFValue amount = new AFValue()
            {
                Id = Guid.NewGuid(),
                AddFieldId = amountOfContainersAffected.Id,
                RequestId = request.Id,
                Value = "0"
            };

            var requestRepositoryMock = new Mock<IRepository<Request>>(MockBehavior.Strict);
            var aFValueRepositoryMock = new Mock<IRepository<AFValue>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            requestRepositoryMock.Setup(m => m.GetAll()).Returns(requestsInDB);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);
            aFValueRepositoryMock.Setup(m => m.Add(It.IsAny<AFValue>()));
            aFValueRepositoryMock.Setup(m => m.SaveChanges());

            requestLogic = new RequestLogic(requestRepositoryMock.Object,
                aFValueRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            aFValueRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetRequestCaseRequestExist()
        {
            var requestRepositoryMock = new Mock<IRepository<Request>>(MockBehavior.Strict);
            var aFValueRepositoryMock = new Mock<IRepository<AFValue>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(request);

            requestLogic = new RequestLogic(requestRepositoryMock.Object,
                aFValueRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Get(request.Id);

            requestRepositoryMock.VerifyAll();
            aFValueRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();

            Assert.AreEqual(result, request);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Invalid ID, Request does not exist")]
        public void GetRequestCaseRequestNotExist()
        {
            var requestRepositoryMock = new Mock<IRepository<Request>>(MockBehavior.Strict);
            var aFValueRepositoryMock = new Mock<IRepository<AFValue>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns((Request)null);

            requestLogic = new RequestLogic(requestRepositoryMock.Object,
                aFValueRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Get(request.Id);

            requestRepositoryMock.VerifyAll();
            aFValueRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetAllRequests()
        {
            var requestRepositoryMock = new Mock<IRepository<Request>>(MockBehavior.Strict);
            var aFValueRepositoryMock = new Mock<IRepository<AFValue>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);

            List<Request> requests = new List<Request>();
            requests.Add(request);
            requestRepositoryMock.Setup(m => m.GetAll()).Returns(requests);

            requestLogic = new RequestLogic(requestRepositoryMock.Object,
                aFValueRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.GetAll();

            requestRepositoryMock.VerifyAll();
            aFValueRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();

            Assert.AreEqual(result, requests);
        }
    }
}
