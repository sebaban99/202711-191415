using System;

namespace IMMRequest.Domain
{
    public class Request
    {
        public Guid Id {get; set;}
        public int RequestNumber {get; set;}
        public Type Type {get; set;}
        public string Details {get; set;}
        public string Name {get; set;}
        public string Email {get; set;}
        public string Phone {get; set;}
        public string Status {get; set;}
        public string Description {get; set;}

    }
}