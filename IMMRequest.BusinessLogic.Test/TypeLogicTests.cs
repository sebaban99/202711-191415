using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Type = IMMRequest.Domain.Type;
using Range = IMMRequest.Domain.Range;
using IMMRequest.Domain;
using IMMRequest.DataAccess.Interfaces;
using Moq;
using System.Linq.Expressions;

namespace IMMRequest.BusinessLogic.Tests
{
    [TestClass]
    public class TypeLogicTests
    {
        private Type type;
        private TypeLogic typeLogic;
        private Topic topic;

        [TestMethod]
        public void CreateTypeCaseValidTypeWithoutAFNotExistsType()
        {
            topic = new Topic()
            {
                Id = Guid.NewGuid(),
                Types = new List<Type>(),
                Area = new Area(),
                Name = "Acoso sexual"
            };

            type = new Type()
            {
                Id = Guid.NewGuid(),
                Name = "Taxi-Acoso",
                Topic = topic,
                AdditionalFields = new List<AdditionalField>()
                
            };

            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());

            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object,
                addFieldRepositoryMock.Object);
            var result = typeLogic.Create(type);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();

            Assert.AreEqual(result, type);
        }

        [TestMethod]
        public void CreateTypeCaseValidTypeWithAFNotExistsType()
        {
            topic = new Topic()
            {
                Id = Guid.NewGuid(),
                Types = new List<Type>(),
                Area = new Area(),
                Name = "Acoso sexual"
            };

            type = new Type()
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
                Type = type,
                Name = "Matricula",
                Range = new List<Range>()
            };

            af.Type = type;
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            addFieldRepositoryMock.Setup(m => m.Add(It.IsAny<AdditionalField>()));
            addFieldRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object,
                addFieldRepositoryMock.Object);
            var result = typeLogic.Create(type);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();

            Assert.AreEqual(result, type);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Topic does not exist")]
        public void CreateTypeCaseInvalidTypeNotExistsTopic()
        {
            topic = new Topic()
            {
                Id = Guid.NewGuid(),
                Types = new List<Type>(),
                Area = new Area(),
                Name = "Acoso sexual"
            };

            type = new Type()
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
                Type = type,
                Name = "Matricula",
                Range = new List<Range>()
            };

            af.Type = type;
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns((Topic)null);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            addFieldRepositoryMock.Setup(a => a.Add(It.IsAny<AdditionalField>()));
            addFieldRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object,
                addFieldRepositoryMock.Object);
            var result = typeLogic.Create(type);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Type with same name already registered")]
        public void CreateTypeCaseInvalidTypeAlreadyExists()
        {
            topic = new Topic()
            {
                Id = Guid.NewGuid(),
                Types = new List<Type>(),
                Area = new Area(),
                Name = "Acoso sexual"
            };

            type = new Type()
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
                Type = type,
                Name = "Matricula",
                Range = new List<Range>()
            };

            af.Type = type;
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns(type);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            addFieldRepositoryMock.Setup(a => a.Add(It.IsAny<AdditionalField>()));
            addFieldRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object,
                addFieldRepositoryMock.Object);
            var result = typeLogic.Create(type);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Type had empty fields")]
        public void CreateTypeCaseInvalidTypeEmptyName()
        {
            topic = new Topic()
            {
                Id = Guid.NewGuid(),
                Types = new List<Type>(),
                Area = new Area(),
                Name = "Acoso sexual"
            };

            type = new Type()
            {
                Id = Guid.NewGuid(),
                Name = "",
                Topic = topic,
                AdditionalFields = new List<AdditionalField>()

            };

            AdditionalField af = new AdditionalField()
            {
                Id = Guid.NewGuid(),
                FieldType = FieldType.Entero,
                Type = type,
                Name = "Matricula",
                Range = new List<Range>()
            };

            af.Type = type;
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            addFieldRepositoryMock.Setup(a => a.Add(It.IsAny<AdditionalField>()));
            addFieldRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object,
                addFieldRepositoryMock.Object);
            var result = typeLogic.Create(type);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Type had empty fields")]
        public void CreateTypeCaseInvalidTypeEmptyTopic()
        {
            topic = new Topic()
            {
                Id = Guid.NewGuid(),
                Types = new List<Type>(),
                Area = new Area(),
                Name = "Acoso sexual"
            };

            type = new Type()
            {
                Id = Guid.NewGuid(),
                Name = "Taxi-Acoso",
                Topic = null,
                AdditionalFields = new List<AdditionalField>()

            };

            AdditionalField af = new AdditionalField()
            {
                Id = Guid.NewGuid(),
                FieldType = FieldType.Entero,
                Type = type,
                Name = "Matricula",
                Range = new List<Range>()
            };

            af.Type = type;
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            addFieldRepositoryMock.Setup(m => m.Add(It.IsAny<AdditionalField>()));
            addFieldRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object,
                addFieldRepositoryMock.Object);
            var result = typeLogic.Create(type);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: AdditionalField had empty fields")]
        public void CreateTypeCaseInvalidTypeInvalidAF()
        {
            topic = new Topic()
            {
                Id = Guid.NewGuid(),
                Types = new List<Type>(),
                Area = new Area(),
                Name = "Acoso sexual"
            };

            type = new Type()
            {
                Id = Guid.NewGuid(),
                Name = "Taxi-Acoso",
                Topic = topic,
                AdditionalFields = new List<AdditionalField>()

            };

            AdditionalField af = new AdditionalField()
            {
                Id = Guid.NewGuid(),
                Type = null,
                Name = "",
                Range = null
            };

            af.Type = type;
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            addFieldRepositoryMock.Setup(m => m.Add(It.IsAny<AdditionalField>()));
            addFieldRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object,
                addFieldRepositoryMock.Object);
            var result = typeLogic.Create(type);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetTypeCaseExistsType()
        {
            topic = new Topic()
            {
                Id = Guid.NewGuid(),
                Types = new List<Type>(),
                Area = new Area(),
                Name = "Acoso sexual"
            };

            type = new Type()
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
                Type = type,
                Name = "Matricula",
                Range = new List<Range>()
            };

            af.Type = type;
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);

            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object,
                addFieldRepositoryMock.Object);
            var result = typeLogic.Get(type.Id);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();

            Assert.AreEqual(result, type);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Invalid ID, Type does not exist")]
        public void GetTypeCaseNotExistsType()
        {
            topic = new Topic()
            {
                Id = Guid.NewGuid(),
                Types = new List<Type>(),
                Area = new Area(),
                Name = "Acoso sexual"
            };

            type = new Type()
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
                Type = type,
                Name = "Matricula",
                Range = new List<Range>()
            };

            af.Type = type;
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns((Type)null);

            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object,
                addFieldRepositoryMock.Object);
            var result = typeLogic.Get(type.Id);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }


        [TestMethod]
        public void GetAllTypes()
        {
            topic = new Topic()
            {
                Id = Guid.NewGuid(),
                Types = new List<Type>(),
                Area = new Area(),
                Name = "Acoso sexual"
            };

            type = new Type()
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
                Type = type,
                Name = "Matricula",
                Range = new List<Range>()
            };

            af.Type = type;
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            List<Type> types = new List<Type>();
            types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            typeRepositoryMock.Setup(m => m.GetAll()).Returns(types);

            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object,
                addFieldRepositoryMock.Object);
            var result = typeLogic.GetAll();

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();

            Assert.AreEqual(result, types);
        }

        [TestMethod]
        public void DeleteTypeCaseExistsType()
        {
            topic = new Topic()
            {
                Id = Guid.NewGuid(),
                Types = new List<Type>(),
                Area = new Area(),
                Name = "Acoso sexual"
            };

            type = new Type()
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
                Type = type,
                Name = "Matricula",
                Range = new List<Range>()
            };

            af.Type = type;
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);
            typeRepositoryMock.Setup(m => m.Remove(type));
            typeRepositoryMock.Setup(m => m.SaveChanges());

            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object,
                addFieldRepositoryMock.Object);
            typeLogic.Remove(type);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Type to delete doesn't exist")]
        public void DeleteTypeCaseNotExistsType()
        {
            topic = new Topic()
            {
                Id = Guid.NewGuid(),
                Types = new List<Type>(),
                Area = new Area(),
                Name = "Acoso sexual"
            };

            type = new Type()
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
                Type = type,
                Name = "Matricula",
                Range = new List<Range>()
            };

            af.Type = type;
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<IRepository<Type>>(MockBehavior.Strict);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.Remove(type));
            typeRepositoryMock.Setup(m => m.SaveChanges());

            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object,
                addFieldRepositoryMock.Object);
            typeLogic.Remove(type);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }
    }
}
