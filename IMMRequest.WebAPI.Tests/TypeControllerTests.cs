using IMMRequest.BusinessLogic;
using IMMRequest.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Range = IMMRequest.Domain.Range;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.WebApi.Tests
{
    [TestClass]
    public class TypeControllerTests
    {
        [TestMethod]
        public void GetAllTypesCaseEmpty()
        {
            var types = new List<Type>();
            var typeLogicMock = new Mock<ITypeLogic>(MockBehavior.Strict);

            typeLogicMock.Setup(m => m.GetAll()).Returns(types);
            var typeController = new TypeController(typeLogicMock.Object);

            var result = typeController.Get();
            var okResult = result as OkObjectResult;
            var value = okResult.Value as List<TypeDTO>;

            typeLogicMock.VerifyAll();
            for (int i = 0; i < types.Count; i++)
            {
                Assert.AreEqual(value[i], types[i]);
            }
        }

        [TestMethod]
        public void GetAllTypesCaseNotEmpty()
        {
            Topic topic = new Topic()
            {
                Id = Guid.NewGuid(),
                Types = new List<Type>(),
                Area = new Area(),
                Name = "Acoso sexual"
            };

            Type oneType = new Type()
            {
                Id = Guid.NewGuid(),
                Name = "Taxi-Acoso",
                Topic = topic,
                AdditionalFields = new List<AdditionalField>()
            };

            Type anotherType = new Type()
            {
                Id = Guid.NewGuid(),
                Name = "Espacio publico-Acoso",
                Topic = topic,
                AdditionalFields = new List<AdditionalField>()
            };

            AdditionalField af = new AdditionalField()
            {
                Id = Guid.NewGuid(),
                FieldType = FieldType.Entero,
                Type = oneType,
                Name = "Matricula",
                Range = new List<Range>()
            };

            af.Type = oneType;
            oneType.AdditionalFields.Add(af);
            topic.Types.Add(oneType);
            topic.Types.Add(anotherType);

            var types = new List<Type>();
            types.Add(oneType);
            types.Add(anotherType);
            var typeLogicMock = new Mock<ITypeLogic>(MockBehavior.Strict);

            typeLogicMock.Setup(m => m.GetAll()).Returns(types);
            var typeController = new TypeController(typeLogicMock.Object);
            var typeModels = new List<TypeDTO>();
            foreach (Type type in types)
            {
                TypeDTO tm = new TypeDTO(type);
                typeModels.Add(tm);
            }

            var result = typeController.Get();
            var okResult = result as OkObjectResult;
            var value = okResult.Value as List<TypeDTO>;

            typeLogicMock.VerifyAll();
            for (int i = 0; i < typeModels.Count; i++)
            {
                Assert.AreEqual(value[i], typeModels[i]);
            }
        }

        [TestMethod]
        public void GetTypeByIdCaseExist()
        {
            Topic topic = new Topic()
            {
                Id = Guid.NewGuid(),
                Types = new List<Type>(),
                Area = new Area(),
                Name = "Acoso sexual"
            };

            Type oneType = new Type()
            {
                Id = Guid.NewGuid(),
                Name = "Taxi-Acoso",
                Topic = topic,
                AdditionalFields = new List<AdditionalField>()
            };

            AdditionalField af = new AdditionalField()
            {
                Id = Guid.NewGuid(),
                FieldType = FieldType.Entero,
                Type = oneType,
                Name = "Matricula",
                Range = new List<Range>()
            };

            af.Type = oneType;
            oneType.AdditionalFields.Add(af);
            topic.Types.Add(oneType);
            var typeDTO = new TypeDTO(oneType);

            var typeLogicMock = new Mock<ITypeLogic>(MockBehavior.Strict);

            typeLogicMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(oneType);
            var typeController = new TypeController(typeLogicMock.Object);

            var result = typeController.Get(oneType.Id);
            var okResult = result as OkObjectResult;
            var value = okResult.Value as TypeDTO;

            typeLogicMock.VerifyAll();

            Assert.AreEqual(typeDTO, value);
        }

        [TestMethod]
        public void GetTypeByIdCaseNotExist()
        {
            Topic topic = new Topic()
            {
                Id = Guid.NewGuid(),
                Types = new List<Type>(),
                Area = new Area(),
                Name = "Acoso sexual"
            };

            Type oneType = new Type()
            {
                Id = Guid.NewGuid(),
                Name = "Taxi-Acoso",
                Topic = topic,
                AdditionalFields = new List<AdditionalField>()
            };

            AdditionalField af = new AdditionalField()
            {
                Id = Guid.NewGuid(),
                FieldType = FieldType.Entero,
                Type = oneType,
                Name = "Matricula",
                Range = new List<Range>()
            };

            af.Type = oneType;
            oneType.AdditionalFields.Add(af);
            topic.Types.Add(oneType);

            var typeLogicMock = new Mock<ITypeLogic>(MockBehavior.Strict);

            typeLogicMock.Setup(m => m.Get(It.IsAny<Guid>())).Throws(
                new BusinessLogicException("Error: Invalid ID, Type does not exist"));
            var typeController = new TypeController(typeLogicMock.Object);

            var result = typeController.Get(oneType.Id);
            var okResult = result as ObjectResult;
            var value = okResult.Value;

            typeLogicMock.VerifyAll();

            Assert.AreEqual(value, "Error: Invalid ID, Type does not exist");
            Assert.AreEqual(okResult.StatusCode, 404);
        }
    }
}
