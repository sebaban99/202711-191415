using IMMRequest.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        public AdditionalFieldDTO() { }

        public AdditionalFieldDTO(AdditionalField af)
        {
            Id = af.Id;
            Name = af.Name;
            FieldType = af.FieldType;
            Type = af.Type.Id;
            Range = af.Range;
        }

        public AdditionalField ToEntity()
        {
            AdditionalField afAsEntity = new AdditionalField()
            {
                Id = this.Id,
                Name = this.Name,
                FieldType = this.FieldType,
                Range = this.Range
            };
            return afAsEntity;
        }
    }
}
