using IMMRequest.DataAccess.Interfaces;
using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using IMMRequest.Exceptions;
using System.Text.RegularExpressions;

namespace IMMRequest.BusinessLogic
{
    public class TopicValidatorHelper : ITopicValidatorHelper
    {
        private IRepository<Topic> topicRepository;
        private IRepository<Area> areaRespository;

        public TopicValidatorHelper(IRepository<Topic> topicRepositor, IRepository<Area> areaRespository)
        {
            this.topicRepository = topicRepositor;
            this.areaRespository = areaRespository;
        }

        private bool IsValidString(string str)
        {
            return str != null && str.Trim() != string.Empty &&
                (!Regex.IsMatch(str, @"^[0-9]$") &&
                Regex.IsMatch(str, @"^[A-Za-z0-9!()'/.,\s-]{1,}$"));
        }

        public bool AreEmptyFields(Topic topic)
        {
            return !IsValidString(topic.Name) || topic.Area == null;
        }

        public void ValidateEntityObject(Topic topic)
        {
            if (AreEmptyFields(topic))
            {
                throw new BusinessLogicException("Error: Topic had empty fields");
            }
        }

        private bool IsAreaRegistered(Guid id)
        {
            Area areaInDB = areaRespository.Get(id);
            return areaInDB != null;
        }

        private void ValidateArea(Topic topic)
        {
            if (!IsAreaRegistered(topic.Area.Id))
            {
                throw new BusinessLogicException("Error: Area does not exist");
            }
        }

        private bool IsTopicRegistered(Topic topic)
        {
            topic.Area = areaRespository.Get(topic.Area.Id);
            Topic topicInDB = topicRepository.GetByCondition(t => t.Name == topic.Name &&
            t.Area == topic.Area);
            return topicInDB != null;
        }

        private void ValidateTopic(Topic topic)
        {
            if (IsTopicRegistered(topic))
            {
                throw new BusinessLogicException("Error: Topic with same name associated to this area already registered");
            }
        }

        public void ValidateAdd(Topic topic)
        {
            ValidateEntityObject(topic);
            ValidateArea(topic);
            ValidateTopic(topic);
        }
    }
}
