using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMMRequest.DataAccess.Repositories;
using IMMRequest.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace IMMRequest.DataAccess.Tests
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
            Description = "Creada",
            AddFieldValues = new List<AFValue>()
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
            Description = "Creada",
            AddFieldValues = new List<AFValue>()
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
            requestRepositoryInMemory.SaveChanges();

            Assert.IsTrue(requestRepositoryInMemory.GetAll().Contains(juanRequest));
        }

        [TestMethod]
        public void RemoveRequest_ExistentRequest_ShouldRemoveRequestFromDB()
        {

            requestRepositoryInMemory.Add(juanRequest);
            requestRepositoryInMemory.Add(pedroRequest);
            requestRepositoryInMemory.Remove(juanRequest);
            requestRepositoryInMemory.SaveChanges();

            Assert.IsFalse(requestRepositoryInMemory.GetAll().Contains(juanRequest));
        }

        [TestMethod]
        public void GetAllRequest_RequestsInDB_ShouldReturnAllRequestInDB()
        {

            requestRepositoryInMemory.Add(juanRequest);
            requestRepositoryInMemory.Add(pedroRequest);
            requestRepositoryInMemory.SaveChanges();

            Assert.AreEqual(requestRepositoryInMemory.GetAll().Count(), 2);
        }

        [TestMethod]
        public void GetAllRequest_EmptyRequestsTable_ShouldReturnAnEmptyCollection()
        {
            Assert.AreEqual(requestRepositoryInMemory.GetAll().Count(), 0);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void UpdateRequest_InexistentRequest_ShouldReturnAnException()
        {
            requestRepositoryInMemory.Update(juanRequest);
            requestRepositoryInMemory.SaveChanges();
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void RemoveRequest_InexistentRequest_ShouldReturnAnException()
        {
            requestRepositoryInMemory.Remove(juanRequest);
            requestRepositoryInMemory.SaveChanges();
        }

        [TestMethod]
        public void GetRequest_InexistentRequest_ShouldReturnNull()
        {
            Request requestById = requestRepositoryInMemory.Get(juanRequest.Id);
            Assert.IsNull(requestById);
        }

        [TestMethod]
        public void GetRequest_ExistentRequest_ShouldReturnSpecificRequestFromDB()
        {

            requestRepositoryInMemory.Add(juanRequest);
            requestRepositoryInMemory.Add(pedroRequest);
            requestRepositoryInMemory.SaveChanges();

            Assert.AreEqual(requestRepositoryInMemory.Get(pedroRequest.Id), pedroRequest);
        }

        [TestMethod]
        public void GetRequestByCondition_ExistentRequest_ShouldReturnSpecificRequestFromDB()
        {
            requestRepositoryInMemory.Add(juanRequest);
            requestRepositoryInMemory.Add(pedroRequest);
            requestRepositoryInMemory.SaveChanges();

            Assert.AreEqual(requestRepositoryInMemory.GetByCondition(
                r => r.RequestNumber == pedroRequest.RequestNumber), pedroRequest);
        }

        [TestMethod]
        public void GetRequestByCondition_InexistentRequest_ShouldReturnNull()
        {
            Request requestById = requestRepositoryInMemory.GetByCondition(
                r => r.RequestNumber == pedroRequest.RequestNumber);
            Assert.IsNull(requestById);
        }

        [TestMethod]
        public void UpdateRequest_ExistentRequest_ShouldUpdateRequestFromDB()
        {
            requestRepositoryInMemory.Add(juanRequest);
            requestRepositoryInMemory.Add(pedroRequest);
            requestRepositoryInMemory.SaveChanges();

            juanRequest.Status = Status.Revision;

            requestRepositoryInMemory.Update(juanRequest);
            requestRepositoryInMemory.SaveChanges();

            Assert.AreEqual(requestRepositoryInMemory.Get(juanRequest.Id), juanRequest);
        }
    }
}
