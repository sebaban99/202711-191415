using IMMRequest.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IMMRequest.WebApi
{
    public class RequestDTO
    {
        public Guid Id { get; set; }
        public int RequestNumber { get; set; }
        public string Details { get; set; }
        public Guid TypeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Status Status { get; set; }
        public string Description { get; set; }
        public List<AFValueDTO> AddFieldValuesDTOs { get; set; }

        public RequestDTO() { }

        public RequestDTO(Request req)
        {
            Id = req.Id;
            RequestNumber = req.RequestNumber;
            TypeId = req.TypeId;
            Details = req.Details;
            Name = req.Name;
            Email = req.Email;
            Phone = req.Phone;
            Status = req.Status;
            Description = req.Description;
            AddFieldValuesDTOs = new List<AFValueDTO>();
            foreach(AFValue af in req.AddFieldValues)
            {
                AFValueDTO afvDTO = new AFValueDTO(af);
                AddFieldValuesDTOs.Add(afvDTO);
            }
        }

        public Request ToEntity()
        {
            Request reqToReturn = new Request()
            {
                Id = this.Id,
                RequestNumber = this.RequestNumber,
                TypeId = this.TypeId,
                Details = this.Details,
                Name = this.Name,
                Email = this.Email,
                Phone = this.Phone,
                Status = this.Status,
                Description = this.Description,
                AddFieldValues = new List<AFValue>()
            };
            if(AddFieldValuesDTOs != null)
            {
                foreach (AFValueDTO afvDTO in this.AddFieldValuesDTOs)
                {
                    AFValue afv = afvDTO.ToEntity();
                    afv.Request = reqToReturn;
                    reqToReturn.AddFieldValues.Add(afv);
                }
            }
            return reqToReturn;
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                RequestDTO requestDTO = (RequestDTO)obj;
                return this.Id == requestDTO.Id;
            }
        }
    }
}