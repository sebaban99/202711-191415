using System;
using System.Collections.Generic;

namespace IMMRequest.Domain
{
    public class AFValue
    {
        public Guid Id { get; set; }
        public Guid RequestId {get; set;}
        public Guid AddFieldId {get; set;}
        public string Value {get; set;}
    }
}