using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMMRequest.Domain;
using System.Linq;

namespace IMMRequest.DataAccess.Test
{
    [TestClass]
    public class RequestRepositoryTests

    {
        private Request juanRequest = new Request()
        {
            Id = Guid.NewGuid(),
            RequestNumber = 1,
            Type = new IMMRequest.Domain.Type(),
            Details = "El contenedor de la esquina Gral Rivera fue incendiado",
            Name = "Juan Perez",
            Email = "juan@perez.com.uy",
            Phone = "099 123 456",
            Status = Status.Creada,
            Description = "Creada"
        };

        private Request pedroRequest = new Request()
        {
            Id = Guid.NewGuid(),
            RequestNumber = 2,
            Type = new IMMRequest.Domain.Type(),
            Details = "El contenedor de la esquina Gral Rivera fue incendiado",
            Name = "Pedro Perez",
            Email = "pedro@perez.com.uy",
            Phone = "099 999 999",
            Status = Status.Creada,
            Description = "Creada"
        };

        private RequestRepository requestRepositoryInMemory = new RequestRepository(
            ContextFactory.GetNewContext(ContextType.MEMORY));

        [TestInitialize]
        public void Initialize()
        {
            requestRepositoryInMemory.Empty();
        }

        [TestMethod]
        public void AddRequest_ValidRequest_ShouldAddRequestToDB()
        {
            requestRepositoryInMemory.Add(juanRequest);

            Assert.AreEqual(requestRepositoryInMemory.GetAll().Count(), 1);
            Assert.IsTrue(requestRepositoryInMemory.GetAll().Contains(juanRequest));
        }

        [TestMethod]
        public void RemoveRequest_ExistentRequest_ShouldRemoveRequestFromDB()
        {

            requestRepositoryInMemory.Add(juanRequest);
            requestRepositoryInMemory.Add(pedroRequest);
            requestRepositoryInMemory.Remove(juanRequest);

            Assert.AreEqual(requestRepositoryInMemory.GetAll().Count(), 1);
            Assert.IsTrue(requestRepositoryInMemory.GetAll().Contains(pedroRequest));
        }

        [TestMethod]
        public void GetAllRequest_RequestsInDB_ShouldReturnAllRequestInDB()
        {

            requestRepositoryInMemory.Add(juanRequest);
            requestRepositoryInMemory.Add(pedroRequest);

            Assert.IsTrue(requestRepositoryInMemory.GetAll().Contains(pedroRequest));
            Assert.IsTrue(requestRepositoryInMemory.GetAll().Contains(juanRequest));
            Assert.AreEqual(requestRepositoryInMemory.GetAll().Count(), 2);
        }

        [TestMethod]
        public void GetRequest_ExistentRequest_ShouldReturnSpecificRequestFromDB()
        {

            requestRepositoryInMemory.Add(juanRequest);
            requestRepositoryInMemory.Add(pedroRequest);

            Assert.AreEqual(requestRepositoryInMemory.Get(pedroRequest.Id), pedroRequest);
        }

        [TestMethod]
        public void GetRequestByCondition_ExistentRequest_ShouldReturnSpecificRequestFromDB()
        {
            requestRepositoryInMemory.Add(juanRequest);
            requestRepositoryInMemory.Add(pedroRequest);

            Assert.AreEqual(requestRepositoryInMemory.GetByCondition(
                r => r.RequestNumber == pedroRequest.RequestNumber), pedroRequest);
        }
    }
}
}
