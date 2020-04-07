using System;
using System.ComponentModel;

namespace IMMRequest.Domain
{
    public enum Status
    {
        Creada,
        [Description("En revisión")]
        Revision,
        Aceptada, 
        Denegada, 
        Finalizada
    }
    public class Request
    {
        public Guid Id {get; set;}
        public int RequestNumber {get; set;}
        public Type Type {get; set;}
        public string Details {get; set;}
        public string Name {get; set;}
        public string Email {get; set;}
        public string Phone {get; set;}
        public Status Status {get; set;}
        public string Description {get; set;}

    }
}