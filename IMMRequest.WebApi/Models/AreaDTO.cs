using IMMRequest.Domain;
using System;
using System.Collections.Generic;

namespace IMMRequest.WebApi
{
    public class AreaDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<TopicDTO> Topics { get; set; }

        public AreaDTO() { }

        public AreaDTO(Area area)
        {
            Id = area.Id;
            Name = area.Name;
            Topics = new List<TopicDTO>();
            foreach (Topic topic in area.Topics)
            {
                TopicDTO topicDTO = new TopicDTO(topic);
                Topics.Add(topicDTO);
            }
        }

        public Area ToEntity()
        {
            Area areaAsEntity = new Area()
            {
                Id = this.Id,
                Name = this.Name,
                Topics = new List<Topic>()
            };
            if (Topics != null)
            {
                foreach (TopicDTO topicDTO in Topics)
                {
                    Topic topic = topicDTO.ToEntity();
                    topic.Area = areaAsEntity;
                    areaAsEntity.Topics.Add(topic);
                }
            }
            return areaAsEntity;
        }
    }
}