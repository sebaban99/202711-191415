using System;
using System.Collections.Generic;
using IMMRequest.DataAccess.Interfaces;
using IMMRequest.Exceptions;
using Type = IMMRequest.Domain.Type;
using AFRangeItem = IMMRequest.Domain.AFRangeItem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMMRequest.Domain;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace IMMRequest.BusinessLogic.Tests
{
    [ExcludeFromCodeCoverage]
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
            AdditionalFields = new List<AdditionalField>(),
            IsActive = true
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

        static AdditionalField dateOfDiscovery = new AdditionalField()
        {
            Id = Guid.NewGuid(),
            FieldType = FieldType.Fecha,
            Type = type,
            Name = "Fecha de descubrimiento",
            Range = new List<AFRangeItem>()
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

        static AFValue stateOfContainerValue = new AFValue()
        {
            Request = request,
            AdditionalField = stateOfContainer,
            Values = new List<AFValueItem>()
        };

        AFValueItem afv1 = new AFValueItem()
        {
            Value = "Incendiado o chocado",
            AFValue = stateOfContainerValue,
            Id = Guid.NewGuid()
        };

        AFValueItem afv2 = new AFValueItem()
        {
            Value = DateTime.Today.ToShortDateString(),
            AFValue = dateOfDiscoveryValue,
            Id = Guid.NewGuid()
        };

        static AFValue dateOfDiscoveryValue = new AFValue()
        {
            Request = request,
            AdditionalField = dateOfDiscovery,
            Values = new List<AFValueItem>()
        };

        List<Request> requestsInDB = new List<Request>();
        private RequestLogic requestLogic;

        [TestInitialize]
        public void Initialize()
        {
            stateOfContainerValue.Values.Add(afv1);
            dateOfDiscoveryValue.Values.Add(afv2);
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
            stateOfContainerValue.Values = new List<AFValueItem>();
            stateOfContainerValue.Values = new List<AFValueItem>();
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

        //[TestMethod]
        //public void CreateRequestCaseValidRequestWithAddFields()
        //{
        //    var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
        //    var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
        //    requestRepositoryMock.Setup(m => m.Add(request));
        //    requestRepositoryMock.Setup(m => m.SaveChanges());
        //    requestRepositoryMock.Setup(m => m.GetAmountOfElements()).Returns(1);
        //    typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);

        //    requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);
        //    var result = requestLogic.Create(request);

        //    requestRepositoryMock.VerifyAll();
        //    typeRepositoryMock.VerifyAll();

        //    Assert.AreEqual(result, request.RequestNumber);
        //}

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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
            };

            trashContainers.Types.Add(brokenContainer);

            Request aRequest = new Request()
            {
                Id = Guid.NewGuid(),
                Type = brokenContainer,
                Details = "El contenedor de",
                Name = "Juan Perez",
                Email = "juan@perez.com.uy",
                Phone = "099 123 456",
                AddFieldValues = new List<AFValue>()
            };

            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.GetAmountOfElements()).Returns(1);
            requestRepositoryMock.Setup(m => m.Add(aRequest));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(brokenContainer);

            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(aRequest);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();

            Assert.AreEqual(result, aRequest.RequestNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Request had empty fields")]
        public void CreateRequestCaseInvalidRequestEmptyName()
        {
            request.Name = "";

            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);

            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Request had empty fields")]
        public void CreateRequestCaseInvalidRequestEmptyDetail()
        {
            request.Details = "";

            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);

            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Request's Type does not exist")]
        public void CreateRequestCaseInvalidRequestNotExistsType()
        {
            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns((Type)null);

            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Request had empty fields")]
        public void CreateRequestCaseInvalidRequestEmptyMail()
        {
            request.Email = "";

            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);

            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Request had empty fields")]
        public void CreateRequestCaseInvalidRequestEmptyType()
        {
            request.Type = null;
            request.AddFieldValues.Clear();

            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns((Type)null);

            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: One Request's additional field value was empty")]
        public void CreateRequestCaseInvalidRequestNullAFV()
        {
            request.AddFieldValues[0] = null;

            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);

            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: One Request's additional field value was empty")]
        public void CreateRequestCaseInvalidRequestAFVNUllValue()
        {
            request.AddFieldValues[0].Values = null;

            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);

            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: One Request's additional field value was invalid, check data type")]
        public void CreateRequestCaseInvalidRequestAFVWrongValueDataTypeScenario1()
        {
            AFValueItem afvItem = new AFValueItem()
            {
                Value = "3 de mayo"
            };
            request.AddFieldValues[1].Values[0] = afvItem;

            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);

            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: One Request's additional field value was invalid, check data type")]
        public void CreateRequestCaseInvalidRequestAFVWrongValueDataTypeScenario2()
        {
            AFValueItem afvItem = new AFValueItem()
            {
                Value = "14"
            };
            request.AddFieldValues[0].Values[0] = afvItem;

            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);

            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: One Request's additional field value was invalid, check fields's range")]
        public void CreateRequestCaseInvalidRequestAFVNotInValidRangeScenario1()
        {
            AFRangeItem yesterday = new AFRangeItem()
            {
                Id = Guid.NewGuid(),
                Value = DateTime.Today.AddDays(-1).ToShortDateString(),
                AdditionalField = dateOfDiscovery
            };

            AFRangeItem tomorrow = new AFRangeItem()
            {
                Id = Guid.NewGuid(),
                Value = DateTime.Today.AddDays(1).ToShortDateString(),
                AdditionalField = dateOfDiscovery
            };

            dateOfDiscovery.Range.Add(yesterday);
            dateOfDiscovery.Range.Add(tomorrow);

            AFValueItem afvItem = new AFValueItem()
            {
                Value = DateTime.Today.AddDays(2).ToShortDateString()
            };
            request.AddFieldValues[1].Values[0] = afvItem;

            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);

            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: One Request's additional field value was invalid, check fields's range")]
        public void CreateRequestCaseInvalidRequestAFVNotInValidRangeScenario2()
        {
            AFValueItem afvItem = new AFValueItem()
            {
                Value = "robado"
            };
            request.AddFieldValues[0].Values[0] = afvItem;

            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Add(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);

            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Create(request);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }


        [TestMethod]
        public void GetRequestCaseRequestExist()
        {
            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(request);

            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Get(request.Id);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();

            Assert.AreEqual(result, request);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Invalid ID, Request does not exist")]
        public void GetRequestCaseRequestNotExist()
        {
            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns((Request)null);

            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.Get(request.Id);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetAllRequests()
        {
            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);

            List<Request> requests = new List<Request>();
            requests.Add(request);
            requestRepositoryMock.Setup(m => m.GetAll()).Returns(requests);

            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);
            var result = requestLogic.GetAll();

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();

            Assert.AreEqual(result, requests);
        }

        [TestMethod]
        public void UpdateRequestCaseValidRequest()
        {
            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(request);
            requestRepositoryMock.Setup(m => m.Update(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());

            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);
            
            Request stubRequest = new Request()
            {
                Id = request.Id,
                Status = Status.Revision,
                Description = "Descripcion en proceso de revision comienza 13/4/20"
            };

            var result = requestLogic.Update(stubRequest);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();

            Assert.AreEqual(result, request);
        }

        [TestMethod]
        public void UpdateRequestCaseValidRequestNewDescriptionOnly()
        {
            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(request);
            requestRepositoryMock.Setup(m => m.Update(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            
            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);
            
            Request stubRequest = new Request()
            {
                Id = request.Id,
                Description = "Descripcion en proceso de revision comienza 13/4/20"
            };

            var result = requestLogic.Update(stubRequest);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();

            Assert.AreEqual(result, request);
        }

        [TestMethod]
        public void UpdateRequestCaseValidRequestNewStatusScenario1()
        {
            request.Status = Status.Finalizada;
            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(request);
            requestRepositoryMock.Setup(m => m.Update(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());
            
            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);
            
            Request stubRequest = new Request()
            {
                Id = request.Id,
                Status = Status.Aceptada,
                Description = request.Description
            };

            var result = requestLogic.Update(stubRequest);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();

            Assert.AreEqual(result, request);
        }

        [TestMethod]
        public void UpdateRequestCaseValidRequestNewStatusScenario2()
        {
            request.Status = Status.Finalizada;
            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(request);
            requestRepositoryMock.Setup(m => m.Update(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());

            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);

            Request stubRequest = new Request()
            {
                Id = request.Id,
                Status = Status.Denegada,
                Description = request.Description
            };

            var result = requestLogic.Update(stubRequest);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();

            Assert.AreEqual(result, request);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Invalid Description update, Request's new description was empty")]
        public void UpdateRequestCaseInvalidRequestEmptyNewDescription()
        {
            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(request);
            requestRepositoryMock.Setup(m => m.Update(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());

            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);

            Request stubRequest = new Request()
            {
                Id = request.Id,
                Status = Status.Revision,
                Description = ""
            };
            
            var result = requestLogic.Update(stubRequest);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Invalid ID, Request does not exist")]
        public void UpdateRequestCaseInvalidRequestNotExists()
        {
            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns((Request)null);
            requestRepositoryMock.Setup(m => m.Update(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());

            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);

            Request stubRequest = new Request()
            {
                Id = request.Id,
                Status = Status.Revision,
                Description = "Descripcion en proceso de revision comienza 13/4/20"
            };

            var result = requestLogic.Update(stubRequest);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }
  
        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Invalid Status update, Request's new status must be next or prior to old status")]
        public void UpdateRequestCaseInvalidRequestStatus()
        {
            var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            requestRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(request);
            requestRepositoryMock.Setup(m => m.Update(request));
            requestRepositoryMock.Setup(m => m.SaveChanges());

            requestLogic = new RequestLogic(requestRepositoryMock.Object, typeRepositoryMock.Object);

            Request stubRequest = new Request()
            {
                Id = request.Id,
                Status = Status.Aceptada,
                Description = "Descripcion en proceso de revision comienza 13/4/20"
            };

            var result = requestLogic.Update(stubRequest);

            requestRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }
    }
}
