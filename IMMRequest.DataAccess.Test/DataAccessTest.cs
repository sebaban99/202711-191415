using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMMRequest.Domain;

namespace IMMRequest.DataAccess.Test
{
    [TestClass]
    public class DataAccessTest

    {
        private Request r1, r2;
        private RequestRepository<Request> requestRepository;

        [TestInitialize]
        public void Initialize()
        {
            requestRepository = new RequestRepository<Request>();

            r1 = new Request(){
                Id = Guid.NewGuid(),
                RequestNumber = 1,
                Type = new IMMRequest.Domain.Type(),
                Details = "detalis request1",
                Name = "Sebastian",
                Email = "seba@immrequest.com",
                Phone = "099 123 456",
                Status = "Creada",
                Description = "Descripcion para test1"
            };

            r2 = new Request(){
                Id = Guid.NewGuid(),
                RequestNumber = 1,
                Type = new Type(),
                Details = "detalis request2",
                Name = "Alejandro",
                Email = "ale@immrequest.com",
                Phone = "099 987 654",
                Status = "Denegada",
                Description = "Descripcion para test2"
            };

        }

        [TestMethod]
        public void AddRequestOK(){
            requestRepository.Add(r1);
            Guid result = r1.Get(r1.Id).Id;
            Guid expected = r1.Id;
            Assert.AreEqual(result, expected);
        }

    }
}
