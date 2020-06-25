using System;
using System.Collections.Generic;
using System.Text;
using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.DataAccess.Interfaces;
using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic
{
    public class TopicLogic : ITopicLogic
    {
        private IRepository<Area> areaRepository;
        private IRepository<Topic> topicRepository;
        private ITopicValidatorHelper topicValidator;

        public TopicLogic(IRepository<Area> areaRepository, IRepository<Topic> topicRepository)
        {
            this.areaRepository = areaRepository;
            this.topicRepository = topicRepository;
            topicValidator = new TopicValidatorHelper(topicRepository, areaRepository);
        }

        private void GiveNewTopicFormat(Topic topic)
        {
            Area realEntity = areaRepository.Get(topic.Area.Id);
            topic.Area = realEntity;
            topic.Id = Guid.NewGuid();
        }

        public Topic Create(Topic topic)
        {
            topicValidator.ValidateAdd(topic);
            GiveNewTopicFormat(topic);
            topicRepository.Add(topic);
            topicRepository.SaveChanges();
            return topic;
        }
    }
}
