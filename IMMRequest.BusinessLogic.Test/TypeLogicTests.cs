using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using IMMRequest.Exceptions;
using Type = IMMRequest.Domain.Type;
using Range = IMMRequest.Domain.Range;
using IMMRequest.Domain;
using IMMRequest.DataAccess.Interfaces;
using Moq;
using System.Linq.Expressions;
using System.Diagnostics.CodeAnalysis;

namespace IMMRequest.BusinessLogic.Tests
{
    [ExcludeFromCodeCoverage]
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
                Name = "Taxi Acoso",
                Topic = topic,
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
            };

            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            var rangeRepositoryMock = new Mock<IRepository<Range>>(MockBehavior.Strict);
            rangeRepositoryMock.Setup(m => m.Add(It.IsAny<Range>()));
            rangeRepositoryMock.Setup(m => m.SaveChanges());
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());

            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
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

            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
           
            var rangeRepositoryMock = new Mock<IRepository<Range>>(MockBehavior.Strict);
            rangeRepositoryMock.Setup(m => m.Add(It.IsAny<Range>()));
            rangeRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
            var result = typeLogic.Create(type);

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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
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

            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns((Topic)null);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            
            var rangeRepositoryMock = new Mock<IRepository<Range>>(MockBehavior.Strict);
            rangeRepositoryMock.Setup(m => m.Add(It.IsAny<Range>()));
            rangeRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
            var result = typeLogic.Create(type);

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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
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
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns(type);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            addFieldRepositoryMock.Setup(a => a.Add(It.IsAny<AdditionalField>()));
            addFieldRepositoryMock.Setup(m => m.SaveChanges());
            var rangeRepositoryMock = new Mock<IRepository<Range>>(MockBehavior.Strict);
            rangeRepositoryMock.Setup(m => m.Add(It.IsAny<Range>()));
            rangeRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
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
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            addFieldRepositoryMock.Setup(a => a.Add(It.IsAny<AdditionalField>()));
            addFieldRepositoryMock.Setup(m => m.SaveChanges());
            var rangeRepositoryMock = new Mock<IRepository<Range>>(MockBehavior.Strict);
            rangeRepositoryMock.Setup(m => m.Add(It.IsAny<Range>()));
            rangeRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
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
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            addFieldRepositoryMock.Setup(m => m.Add(It.IsAny<AdditionalField>()));
            addFieldRepositoryMock.Setup(m => m.SaveChanges());
            var rangeRepositoryMock = new Mock<IRepository<Range>>(MockBehavior.Strict);
            rangeRepositoryMock.Setup(m => m.Add(It.IsAny<Range>()));
            rangeRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
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
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            addFieldRepositoryMock.Setup(m => m.Add(It.IsAny<AdditionalField>()));
            addFieldRepositoryMock.Setup(m => m.SaveChanges());
            var rangeRepositoryMock = new Mock<IRepository<Range>>(MockBehavior.Strict);
            rangeRepositoryMock.Setup(m => m.Add(It.IsAny<Range>()));
            rangeRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
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
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);
            var rangeRepositoryMock = new Mock<IRepository<Range>>(MockBehavior.Strict);


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
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
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns((Type)null);

            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
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
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            typeRepositoryMock.Setup(m => m.GetActiveTypes()).Returns(types);

            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
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
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(type);
            typeRepositoryMock.Setup(m => m.SoftDelete(type));
            typeRepositoryMock.Setup(m => m.SaveChanges());

            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
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
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            typeRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.Remove(type));
            typeRepositoryMock.Setup(m => m.SaveChanges());

            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
            typeLogic.Remove(type);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Taxi-Acoso's range has invalid text values")]
        public void CreateTypeCaseInvalidAFRangeTextScenario1()
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
            };

            AdditionalField af = new AdditionalField()
            {
                Id = Guid.NewGuid(),
                Type = type,
                Name = "Empresa de taxi",
                FieldType = FieldType.Texto,
                Range = new List<Range>()
            };

            Range option1 = new Range()
            {
                Value = "Radio Taxi",
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            Range option2 = new Range()
            {
                Value = "Fono Taxi",
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            Range option3 = new Range()
            {
                Value = "1",
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            af.Range.Add(option1);
            af.Range.Add(option2);
            af.Range.Add(option3);
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            addFieldRepositoryMock.Setup(m => m.Add(It.IsAny<AdditionalField>()));
            addFieldRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
            var result = typeLogic.Create(type);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void CreateTypeCaseValidAFRangeTextScenario2()
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
            };

            AdditionalField af = new AdditionalField()
            {
                Id = Guid.NewGuid(),
                Type = type,
                Name = "Empresa de taxi",
                FieldType = FieldType.Texto,
                Range = new List<Range>()
            };

            Range option1 = new Range()
            {
                Value = "Radio Taxi",
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            Range option2 = new Range()
            {
                Value = "Fono Taxi",
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            Range option3 = new Range()
            {
                Value = "Taxi aeropuerto",
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            af.Range.Add(option1);
            af.Range.Add(option2);
            af.Range.Add(option3);
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
            var result = typeLogic.Create(type);

            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Taxi-Acoso's range has invalid integer values")]
        public void CreateTypeCaseInvalidAFRangeIntegerScenario1()
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
            };

            AdditionalField af = new AdditionalField()
            {
                Id = Guid.NewGuid(),
                Type = type,
                Name = "Empresa de taxi",
                FieldType = FieldType.Entero,
                Range = new List<Range>()
            };

            Range option2 = new Range()
            {
                Value = "3",
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            Range option1 = new Range()
            {
                Value = "1",
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            Range option3 = new Range()
            {
                Value = "5",
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            af.Range.Add(option1);
            af.Range.Add(option2);
            af.Range.Add(option3);
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            addFieldRepositoryMock.Setup(m => m.Add(It.IsAny<AdditionalField>()));
            addFieldRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
            var result = typeLogic.Create(type);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Taxi-Acoso's range has invalid integer values")]
        public void CreateTypeCaseInvalidAFRangeIntegerScenario2()
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
            };

            AdditionalField af = new AdditionalField()
            {
                Id = Guid.NewGuid(),
                Type = type,
                Name = "Empresa de taxi",
                FieldType = FieldType.Entero,
                Range = new List<Range>()
            };

            Range option2 = new Range()
            {
                Value = "1",
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            Range option1 = new Range()
            {
                Value = "1",
                Id = Guid.NewGuid(),
                AdditionalField = af
            };


            af.Range.Add(option1);
            af.Range.Add(option2);
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            addFieldRepositoryMock.Setup(m => m.Add(It.IsAny<AdditionalField>()));
            addFieldRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
            var result = typeLogic.Create(type);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Taxi-Acoso's range has invalid integer values")]
        public void CreateTypeCaseInvalidAFRangeIntegerScenario3()
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
            };

            AdditionalField af = new AdditionalField()
            {
                Id = Guid.NewGuid(),
                Type = type,
                Name = "Empresa de taxi",
                FieldType = FieldType.Entero,
                Range = new List<Range>()
            };

            Range option1 = new Range()
            {
                Value = "1",
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            af.Range.Add(option1);
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            addFieldRepositoryMock.Setup(m => m.Add(It.IsAny<AdditionalField>()));
            addFieldRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
            var result = typeLogic.Create(type);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Taxi-Acoso's range has invalid integer values")]
        public void CreateTypeCaseInvalidAFRangeIntegerScenario4()
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
            };

            AdditionalField af = new AdditionalField()
            {
                Id = Guid.NewGuid(),
                Type = type,
                Name = "Empresa de taxi",
                FieldType = FieldType.Entero,
                Range = new List<Range>()
            };

            Range option2 = new Range()
            {
                Value = "1",
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            Range option1 = new Range()
            {
                Value = "4",
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            af.Range.Add(option1);
            af.Range.Add(option2);
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            addFieldRepositoryMock.Setup(m => m.Add(It.IsAny<AdditionalField>()));
            addFieldRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
            var result = typeLogic.Create(type);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void CreateTypeCaseValidAFRangeIntegerScenario5()
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
            };

            AdditionalField af = new AdditionalField()
            {
                Id = Guid.NewGuid(),
                Type = type,
                Name = "Empresa de taxi",
                FieldType = FieldType.Entero,
                Range = new List<Range>()
            };

            Range option1 = new Range()
            {
                Value = "1",
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            Range option2 = new Range()
            {
                Value = "4",
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            af.Range.Add(option1);
            af.Range.Add(option2);
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            

            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
            var result = typeLogic.Create(type);

            
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();

            Assert.AreEqual(result, type);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Taxi-Acoso's range has invalid date values")]
        public void CreateTypeCaseInvalidAFRangeDateScenario1()
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
            };

            AdditionalField af = new AdditionalField()
            {
                Id = Guid.NewGuid(),
                Type = type,
                Name = "Empresa de taxi",
                FieldType = FieldType.Fecha,
                Range = new List<Range>()
            };

            DateTime date = DateTime.Today;

            Range option1 = new Range()
            {
                Value = date.ToShortDateString(),
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            Range option2 = new Range()
            {
                Value = date.ToShortDateString(),
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            af.Range.Add(option1);
            af.Range.Add(option2);
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            addFieldRepositoryMock.Setup(m => m.Add(It.IsAny<AdditionalField>()));
            addFieldRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
            var result = typeLogic.Create(type);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Taxi-Acoso's range has invalid date values")]
        public void CreateTypeCaseInvalidAFRangeDateScenario2()
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
            };

            AdditionalField af = new AdditionalField()
            {
                Id = Guid.NewGuid(),
                Type = type,
                Name = "Empresa de taxi",
                FieldType = FieldType.Fecha,
                Range = new List<Range>()
            };

            DateTime date = DateTime.Today;
            Range option1 = new Range()
            {
                Value = date.ToShortDateString(),
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            af.Range.Add(option1);
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            addFieldRepositoryMock.Setup(m => m.Add(It.IsAny<AdditionalField>()));
            addFieldRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
            var result = typeLogic.Create(type);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Taxi-Acoso's range has invalid date values")]
        public void CreateTypeCaseInvalidAFRangeDateScenario3()
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
            };

            AdditionalField af = new AdditionalField()
            {
                Id = Guid.NewGuid(),
                Type = type,
                Name = "Empresa de taxi",
                FieldType = FieldType.Fecha,
                Range = new List<Range>()
            };

            DateTime date1 = DateTime.Today;
            DateTime date2 = date1.AddDays(+1);
            DateTime date3 = date2.AddDays(+1);

            Range option1 = new Range()
            {
                Value = date1.ToShortDateString(),
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            Range option2 = new Range()
            {
                Value = date2.ToShortDateString(),
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            Range option3 = new Range()
            {
                Value = date3.ToShortDateString(),
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            af.Range.Add(option1);
            af.Range.Add(option2);
            af.Range.Add(option3);
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            addFieldRepositoryMock.Setup(m => m.Add(It.IsAny<AdditionalField>()));
            addFieldRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
            var result = typeLogic.Create(type);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Taxi-Acoso's range has invalid date values")]
        public void CreateTypeCaseInvalidAFRangeDateScenario4()
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
            };

            AdditionalField af = new AdditionalField()
            {
                Id = Guid.NewGuid(),
                Type = type,
                Name = "Empresa de taxi",
                FieldType = FieldType.Fecha,
                Range = new List<Range>()
            };

            DateTime date1 = DateTime.Today;
            DateTime date2 = date1.AddDays(-1);

            Range option1 = new Range()
            {
                Value = date1.ToShortDateString(),
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            Range option2 = new Range()
            {
                Value = date2.ToShortDateString(),
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            af.Range.Add(option1);
            af.Range.Add(option2);
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            addFieldRepositoryMock.Setup(m => m.Add(It.IsAny<AdditionalField>()));
            addFieldRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
              
            var result = typeLogic.Create(type);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException), "Error: Taxi-Acoso's range has invalid date values")]
        public void CreateTypeCaseInvalidAFRangeDateScenario5()
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
            };

            AdditionalField af = new AdditionalField()
            {
                Id = Guid.NewGuid(),
                Type = type,
                Name = "Empresa de taxi",
                FieldType = FieldType.Fecha,
                Range = new List<Range>()
            };

            DateTime date1 = DateTime.Today;

            Range option1 = new Range()
            {
                Value = date1.ToShortDateString(),
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            Range option2 = new Range()
            {
                Value = "3 de mayo",
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            af.Range.Add(option1);
            af.Range.Add(option2);
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var addFieldRepositoryMock = new Mock<IRepository<AdditionalField>>(MockBehavior.Strict);
            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
            addFieldRepositoryMock.Setup(m => m.Add(It.IsAny<AdditionalField>()));
            addFieldRepositoryMock.Setup(m => m.SaveChanges());


            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
            var result = typeLogic.Create(type);

            addFieldRepositoryMock.VerifyAll();
            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void CreateTypeCaseValidAFRangeDateScenario6()
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
                AdditionalFields = new List<AdditionalField>(),
                IsActive = true
            };

            AdditionalField af = new AdditionalField()
            {
                Id = Guid.NewGuid(),
                Type = type,
                Name = "Empresa de taxi",
                FieldType = FieldType.Fecha,
                Range = new List<Range>()
            };

            DateTime date1 = DateTime.Today;
            DateTime date2 = date1.AddDays(+1);

            Range option1 = new Range()
            {
                Value = date1.ToShortDateString(),
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            Range option2 = new Range()
            {
                Value = date2.ToShortDateString(),
                Id = Guid.NewGuid(),
                AdditionalField = af
            };

            af.Range.Add(option1);
            af.Range.Add(option2);
            type.AdditionalFields.Add(af);
            topic.Types.Add(type);

            var topicRepositoryMock = new Mock<IRepository<Topic>>(MockBehavior.Strict);
            var typeRepositoryMock = new Mock<ITypeRepository>(MockBehavior.Strict);
            topicRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(topic);
            typeRepositoryMock.Setup(m => m.Add(type));
            typeRepositoryMock.Setup(m => m.GetByCondition(
                It.IsAny<Expression<Func<Type, bool>>>())).Returns((Type)null);
            typeRepositoryMock.Setup(m => m.SaveChanges());
           
            typeLogic = new TypeLogic(typeRepositoryMock.Object, topicRepositoryMock.Object);
            var result = typeLogic.Create(type);

            topicRepositoryMock.VerifyAll();
            typeRepositoryMock.VerifyAll();

            Assert.AreEqual(result, type);
        }
    }
}
