using IMMRequest.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMMRequest.WebApi
{
    public class AFValueDTO
    {
        public Guid Id { get; set; }
        public Guid RequestId { get; set; }
        public Guid AdditionalFieldId { get; set; }
        public string Value { get; set; }

        public AFValueDTO() { }

        public AFValueDTO(AFValue afv)
        {
            Id = afv.Id;
            RequestId = afv.Request.Id;
            AdditionalFieldId = afv.AdditionalField.Id;
            Value = afv.Value;
        }

        public AFValue ToEntity()
        {
            AFValue afvAsEntity = new AFValue()
            {
                Id = this.Id,
                Request = new Request()
                {
                    Id = this.RequestId 
                },
                AdditionalField = new AdditionalField()
                {
                    Id = this.AdditionalFieldId
                },
                Value = this.Value
            };
            return afvAsEntity;
        }
    }
}
