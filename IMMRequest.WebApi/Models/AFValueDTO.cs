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
        public List<AFValueItemDTO> ValuesItemDTOs { get; set; }

        public AFValueDTO() { }

        public AFValueDTO(AFValue afv)
        {
            Id = afv.Id;
            RequestId = afv.Request.Id;
            AdditionalFieldId = afv.AdditionalFieldID;
            ValuesItemDTOs = new List<AFValueItemDTO>();
            foreach (AFValueItem afvi in afv.Values)
            {
                AFValueItemDTO AFVItemDTO = new AFValueItemDTO(afvi);
                ValuesItemDTOs.Add(AFVItemDTO);
            }
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
                AdditionalFieldID = this.AdditionalFieldId,
                Values = new List<AFValueItem>()
            };
            foreach (AFValueItemDTO valueItemDTO in this.ValuesItemDTOs)
            {
                AFValueItem valueEntity = valueItemDTO.ToEntity();
                valueEntity.AFValue = afvAsEntity;
                afvAsEntity.Values.Add(valueEntity);
            }
            return afvAsEntity;
        }
    }
}
