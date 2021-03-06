using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace IMMRequest.Domain
{
    public enum Status
    {
        Creada,
        [Description("En revisi�n")]
        Revision,
        Aceptada, 
        Denegada, 
        Finalizada
    }
    public class Request
    {
        public Guid Id {get; set;}
        public int RequestNumber {get; set;}
        public virtual Type Type {get; set;}
        public Guid TypeId { get; set; }
        public string Details {get; set;}
        public string Name {get; set;}
        public string Email {get; set;}
        public string Phone {get; set;}
        public Status Status {get; set;}
        public string Description {get; set;}
        public DateTime CreationDate { get; set; }
        public List<AFValue> AddFieldValues {get; set;}
    }
}