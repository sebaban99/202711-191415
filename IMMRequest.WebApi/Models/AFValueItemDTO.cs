using IMMRequest.Domain;
using System;
using System.Diagnostics.CodeAnalysis;

namespace IMMRequest.WebApi
{
    [ExcludeFromCodeCoverage]
    public class AFValueItemDTO
    {
        public Guid Id { get; set; }
        public Guid AFValueId { get; set; }
        public string Value { get; set; }

        public AFValueItemDTO() { }

        public AFValueItemDTO(AFValueItem afvi)
        {
            Id = afvi.Id;
            AFValueId = afvi.AFValue.Id;
            Value = afvi.Value;
        }

        public AFValueItem ToEntity()
        {
            AFValueItem rangeAsEntity = new AFValueItem()
            {
                Id = this.Id,
                AFValue = new AFValue()
                {
                    Id = this.AFValueId
                },
                Value = this.Value
            };
            return rangeAsEntity;
        }

    }
}
