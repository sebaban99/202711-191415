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
        public TypeDTO Type { get; set; }
        public string Details { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Status Status { get; set; }
        public string Description { get; set; }
        public List<AFValue> AddFieldValues { get; set; }

        public RequestDTO(Request req)
        {
            Id = req.Id;
            RequestNumber = req.RequestNumber;
            Type = new TypeDTO(req.Type);
            Details = req.Details;
            Name = req.Name;
            Email = req.Email;
            Phone = req.Phone;
            Status = req.Status;
            Description = req.Description;
            AddFieldValues = req.AddFieldValues;
        }

        public Request ToEntity()
        {
            Request reqToReturn = new Request()
            {
                Id = this.Id,
                RequestNumber = this.RequestNumber,
                Type = this.Type.ToEntity(),
                Details = this.Details,
                Name = this.Name,
                Email = this.Email,
                Phone = this.Phone,
                Status = this.Status,
                Description = this.Description,
                AddFieldValues = this.AddFieldValues,
            };
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