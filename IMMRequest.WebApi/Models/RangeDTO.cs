using IMMRequest.Domain;
using System;
using Range = IMMRequest.Domain.Range;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace IMMRequest.WebApi
{
    [ExcludeFromCodeCoverage]
    public class RangeDTO
    {
        public Guid Id { get; set; }
        public Guid AdditionalFieldId { get; set; }
        public string Value { get; set; }

        public RangeDTO() { }

        public RangeDTO(Range range)
        {
            Id = range.Id;
            AdditionalFieldId = range.AdditionalField.Id;
            Value = range.Value;
        }
        
        public Range ToEntity()
        {
            Range rangeAsEntity = new Range()
            {
                Id = this.Id,
                AdditionalField = new AdditionalField()
                {
                    Id = this.AdditionalFieldId
                },
                Value = this.Value
            };
            return rangeAsEntity;
        }

    }
}
