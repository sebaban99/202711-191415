using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.DataAccess.Interfaces;
using IMMRequest.Domain;
using IMMRequest.Exceptions;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.BusinessLogic
{
    public class ImportProcessing : IImportProcessing
    {
        private IRepository<Area> areaRepository;
        private IRepository<Topic> topicRepository;
        private ITypeRepository typeRepository;

        public ImportProcessing(IRepository<Area> areaRepository, IRepository<Topic> topicRespository,
            ITypeRepository typeRepository)
        {
            this.areaRepository = areaRepository;
            this.topicRepository = topicRespository;
            this.typeRepository = typeRepository;
        }

        private bool IsValidString(string str)
        {
            return str != null && str.Trim() != string.Empty &&
                (!Regex.IsMatch(str, @"^[0-9]$") &&
                Regex.IsMatch(str, @"^[A-Za-z0-9!()'/.,\s-]{1,}$"));
        }

        private void ProcessTypeFromExistingTopic(Type type)
        {
            if (IsValidString(type.Name))
            {
                Type typeByName = typeRepository.GetByCondition(t => t.Name == type.Name &&
                    t.Topic.Name == type.Topic.Name);
                if (typeByName == null)
                {
                    type.CreationDate = DateTime.Now;
                    type.Id = Guid.NewGuid();
                    type.IsActive = true;
                    type.AdditionalFields = new List<AdditionalField>();
                    type.Topic = topicRepository.GetByCondition(t => t.Name == type.Topic.Name &&
                        t.Area.Name == type.Topic.Area.Name);
                }
                else
                {
                    throw new ImportException($"Error on import: Type: {type.Name} from Topic: {type.Topic.Name} from Area: {type.Topic.Area.Name} already exists");
                }
            }
            else
            {
                throw new ImportException($"Error on import: Type from Topic: {type.Topic.Name} from Area: {type.Topic.Area.Name} was invalid");
            }
        }

        private void ProcessNewTopic(Topic topic)
        {
            topic.Id = Guid.NewGuid();
            topic.Area = areaRepository.GetByCondition(a => a.Name == topic.Area.Name);
            if (topic.Types != null)
            {
                foreach (Type type in topic.Types)
                {
                    if (IsValidString(type.Name))
                    {
                        type.CreationDate = DateTime.Now;
                        type.Id = Guid.NewGuid();
                        type.IsActive = true;
                        type.AdditionalFields = new List<AdditionalField>();
                    }
                    else
                    {
                        throw new ImportException($"Error on import: Topic: {topic.Name} from {topic.Area.Name} had invalid Type");
                    }
                }
            }
            else
            {
                topic.Types = new List<Type>();
            }
        }

        private void ProcessTopicFromExisitngArea(Topic topic)
        {
            if (IsValidString(topic.Name))
            {
                Topic topicByName = topicRepository.GetByCondition(t => t.Name == topic.Name &&
                    t.Area.Name == topic.Area.Name);
                if (topicByName == null)
                {
                    ProcessNewTopic(topic);
                }
                else
                {
                    if (topic.Types == null || topic.Types.Count == 0)
                    {
                        throw new ImportException($"Error on import: Topic: {topic.Name} from Area: {topic.Area.Name} already exists");
                    }
                    else
                    {
                        foreach (Type type in topic.Types)
                        {
                            ProcessTypeFromExistingTopic(type);
                        }
                    }
                }
            }
            else
            {
                throw new ImportException($"Error on import: Area: {topic.Area.Name} had invalid Topic");
            }
        }

        private void ProcessNewArea(Area area)
        {
            area.Id = Guid.NewGuid();
            if (area.Topics != null && area.Topics.Count != 0)
            {
                for (int i = 0; i < area.Topics.Count; i++)
                {
                    Topic oneTopic = area.Topics[i];
                    oneTopic.Id = Guid.NewGuid();
                    if (IsValidString(oneTopic.Name))
                    {
                        if (oneTopic.Types != null)
                        {
                            foreach (Type type in oneTopic.Types)
                            {
                                if (IsValidString(type.Name))
                                {
                                    type.CreationDate = DateTime.Now;
                                    type.Id = Guid.NewGuid();
                                    type.IsActive = true;
                                    type.AdditionalFields = new List<AdditionalField>();
                                }
                                else
                                {
                                    throw new ImportException($"Error on import: Area {area.Name} had invalid Topic");
                                }
                            }
                        }
                        else
                        {
                            oneTopic.Types = new List<Type>();
                        }
                    }
                    else
                    {
                        throw new ImportException($"Error on import: Area {area.Name} had invalid Topic");
                    }
                }
            }
        }

        private void ProcessArea(Area area)
        {
            if (area != null && IsValidString(area.Name))
            {
                Area areaByName = areaRepository.GetByCondition(a => a.Name == area.Name);
                if (areaByName == null)
                {
                    ProcessNewArea(area);
                }
                else
                {
                    if (area.Topics == null || area.Topics.Count == 0)
                    {
                        throw new ImportException($"Error on import: Area {area.Name} already exists");
                    }
                    else
                    {
                        foreach (Topic topic in area.Topics)
                        {
                            ProcessTopicFromExisitngArea(topic);
                        }
                    }
                }
                areaRepository.Add(area);
                areaRepository.SaveChanges();
            }
            else
            {
                throw new ImportException("Error on import: One Area's name was empty or invalid");
            }
        }

        public void ProcessImportedElements(List<Area> elementsToImport)
        {
            foreach(Area area in elementsToImport)
            {
                ProcessArea(area);
                areaRepository.Add(area);
                areaRepository.SaveChanges();
            }
        }
    }
}
