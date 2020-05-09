using IMMRequest.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Range = IMMRequest.Domain.Range;

namespace IMMRequest.WebApi
{
    public class AdditionalFieldDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public FieldType FieldType { get; set; }
        public Guid Type { get; set; }
        public List<Range> Range { get; set; }

        public AdditionalFieldDTO(AdditionalField af)
        {
            Id = af.Id;
            Name = af.Name;
            FieldType = af.FieldType;
            Type = af.Type.Id;
            Range = af.Range;
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                AdditionalFieldDTO afm = (AdditionalFieldDTO)obj;
                return this.Id == afm.Id;
            }
        }
    }
}
