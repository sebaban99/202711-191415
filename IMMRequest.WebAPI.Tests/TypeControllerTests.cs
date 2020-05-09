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
        private static Topic topic = new Topic()
        {
            Id = Guid.NewGuid(),
            Types = new List<Type>(),
            Area = new Area(),
            Name = "Acoso sexual"
        };

        private static Type oneType = new Type()
        {
            Id = Guid.NewGuid(),
            Name = "Taxi-Acoso",
            Topic = topic,
            AdditionalFields = new List<AdditionalField>()
        };

        private static Type anotherType = new Type()
        {
            Id = Guid.NewGuid(),
            Name = "Espacio publico-Acoso",
            Topic = topic,
            AdditionalFields = new List<AdditionalField>()
        };

        private static AdditionalField af = new AdditionalField()
        {
            Id = Guid.NewGuid(),
            FieldType = FieldType.Entero,
            Type = oneType,
            Name = "Matricula",
            Range = new List<Range>()
        };

        [TestInitialize]
        public void Initialize()
        {
            af.Type = oneType;
            oneType.AdditionalFields.Add(af);
            topic.Types.Add(oneType);
            topic.Types.Add(anotherType);
        }

        [TestCleanup]
        public void CleanUp()
        {
            oneType.Id = Guid.NewGuid();
            oneType.Name = "Taxi-Acoso";
            oneType.Topic = topic;
        }

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

        [TestMethod]
        public void PostCaseValidType()
        {
            var typeDTO = new TypeDTO(oneType);
            var typeLogicMock = new Mock<ITypeLogic>(MockBehavior.Strict);

            typeLogicMock.Setup(m => m.Create(It.IsAny<Type>())).Returns(oneType);
            var typeController = new TypeController(typeLogicMock.Object);

            var result = typeController.Post(typeDTO);
            var okResult = result as OkObjectResult;
            var value = okResult.Value;

            typeLogicMock.VerifyAll();

            Assert.AreEqual(typeDTO, value);
        }

        [TestMethod]
        public void PostCaseInvalidTypeAlreadyRegistered()
        {
            var typeDTO = new TypeDTO(oneType);
            var typeLogicMock = new Mock<ITypeLogic>(MockBehavior.Strict);

            typeLogicMock.Setup(m => m.Create(It.IsAny<Type>())).Throws(
                new BusinessLogicException("Error: Type with same name associated to this topic already registered"));
            var typeController = new TypeController(typeLogicMock.Object);

            var result = typeController.Post(typeDTO);
            var okResult = result as ObjectResult;
            var value = okResult.Value;

            typeLogicMock.VerifyAll();

            Assert.AreEqual(value, "Error: Type with same name associated to this topic already registered");
            Assert.AreEqual(okResult.StatusCode, 400);
        }

        [TestMethod]
        public void PostCaseInvalidTypeEmptyFields()
        {
            oneType.Name = "";
            var typeDTO = new TypeDTO(oneType);
            var typeLogicMock = new Mock<ITypeLogic>(MockBehavior.Strict);

            typeLogicMock.Setup(m => m.Create(It.IsAny<Type>())).Throws(
                new BusinessLogicException("Error: Type had empty fields"));
            var typeController = new TypeController(typeLogicMock.Object);

            var result = typeController.Post(typeDTO);
            var okResult = result as ObjectResult;
            var value = okResult.Value;

            typeLogicMock.VerifyAll();

            Assert.AreEqual(value, "Error: Type had empty fields");
            Assert.AreEqual(okResult.StatusCode, 400);
        }
    }
}
