using IMMRequest.Domain; 
using IMMRequest.WebApi; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using Type = IMMRequest.Domain.Type; 
 
namespace IMMRequest.WebApi
{
    public class TypeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid Topic { get; set; }
        public List<AdditionalFieldDTO> aFModels { get; set; }

        public TypeDTO(Type type)
        {
            Id = type.Id;
            Name = type.Name;
            Topic = type.Topic.Id;
            aFModels = new List<AdditionalFieldDTO>();
            foreach (AdditionalField af in type.AdditionalFields)
            {
                AdditionalFieldDTO afm = new AdditionalFieldDTO(af);
                aFModels.Add(afm);
            }
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                TypeDTO tm = (TypeDTO)obj;
                return this.Id == tm.Id;
            }
        }
    }
}