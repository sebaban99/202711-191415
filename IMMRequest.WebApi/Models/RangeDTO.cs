using IMMRequest.Domain;
using System;
using AFRangeItem = IMMRequest.Domain.AFRangeItem;
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

        public RangeDTO(AFRangeItem range)
        {
            Id = range.Id;
            AdditionalFieldId = range.AdditionalField.Id;
            Value = range.Value;
        }
        
        public AFRangeItem ToEntity()
        {
            AFRangeItem rangeAsEntity = new AFRangeItem()
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
