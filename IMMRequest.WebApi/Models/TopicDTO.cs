using IMMRequest.Domain;
using System;
using System.Collections.Generic;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.WebApi
{
    public class TopicDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<TypeDTO> Types { get; set; }

        public TopicDTO() { }

        public TopicDTO(Topic topic)
        {
            Id = topic.Id;
            Name = topic.Name;
            Types = new List<TypeDTO>();
            foreach (Type type in topic.Types)
            {
                TypeDTO typeDTO = new TypeDTO(type);
                Types.Add(typeDTO);
            }
        }

        public Topic ToEntity()
        {
            Topic topicAsEntity = new Topic()
            {
                Id = this.Id,
                Name = this.Name,
                Types = new List<Type>()
            };
            if (Types != null)
            {
                foreach (TypeDTO typeDTO in Types)
                {
                    Type type = typeDTO.ToEntity();
                    type.Topic = topicAsEntity;
                    topicAsEntity.Types.Add(type);
                }
            }
            return topicAsEntity;
        }
    }
}