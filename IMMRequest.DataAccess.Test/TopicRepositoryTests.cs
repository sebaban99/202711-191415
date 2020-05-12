using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using IMMRequest.Domain;
using System.Collections.Generic;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.DataAccess.Tests
{
    [TestClass]
    public class TopicRepositoryTests
    {
        Topic oneTopic= new Topic()
        {
            Id = Guid.NewGuid(),
            Name = "Arbolado",
            Area = new Area(),
            Types = new List<Type>()
        };

        Topic anotherTopic = new Topic()
        {
            Id = Guid.NewGuid(),
            Name = "Otros",
            Area = new Area(),
            Types = new List<Type>()
        };

        private TopicRepository topicRepositoryInMemory = new TopicRepository(
            ContextFactory.GetNewContext(ContextType.MEMORY));

        [TestInitialize]
        public void Initialize()
        {
            topicRepositoryInMemory.Empty();
        }

        [TestMethod]
        public void GetTopic_InexistentTopic_ShouldReturnNull()
        {
            Topic topicById = topicRepositoryInMemory.Get(oneTopic.Id);
            Assert.IsNull(topicById);
        }

        [TestMethod]
        public void GetTopic_ExistentTopic_ShouldReturnSpecificTopicFromDB()
        {
            topicRepositoryInMemory.Add(oneTopic);
            topicRepositoryInMemory.Add(anotherTopic);
            topicRepositoryInMemory.SaveChanges();

            Assert.AreEqual(topicRepositoryInMemory.Get(oneTopic.Id),
                oneTopic);
        }
    }
}
