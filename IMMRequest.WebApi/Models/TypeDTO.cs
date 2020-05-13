using IMMRequest.Domain; 
using IMMRequest.WebApi; 
using System; 
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq; 
using Type = IMMRequest.Domain.Type; 
 
namespace IMMRequest.WebApi
{
    public class TypeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid TopicId { get; set; }
        public bool IsActive { get; set; }
        public List<AdditionalFieldDTO> aFDTOs { get; set; }

        public TypeDTO(Type type)
        {
            Id = type.Id;
            Name = type.Name;
            TopicId = type.Topic.Id;
            IsActive = type.IsActive;
            aFDTOs = new List<AdditionalFieldDTO>();
            foreach (AdditionalField af in type.AdditionalFields)
            {
                AdditionalFieldDTO afDTO = new AdditionalFieldDTO(af);
                aFDTOs.Add(afDTO);
            }
        }

        public Type ToEntity()
        {
            Type typeAsEntity = new Type()
            {
                Id = this.Id,
                Name = this.Name,
                IsActive = this.IsActive,
                Topic = new Topic()
                {
                    Id = this.TopicId
                },
                AdditionalFields = new List<AdditionalField>()
            };
            foreach (AdditionalFieldDTO afDTO in this.aFDTOs)
            {
                AdditionalField af = afDTO.ToEntity();
                af.Type = typeAsEntity;
                typeAsEntity.AdditionalFields.Add(af);
            }
            return typeAsEntity;
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                TypeDTO typeDTO = (TypeDTO)obj;
                return this.Id == typeDTO.Id;
            }
        }
    }
}