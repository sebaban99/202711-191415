using System;
using System.Collections.Generic;

namespace IMMRequest.Domain
{
    public class Type
    {
        public Guid Id {get; set;}
        public string Name {get; set;}
        public Topic Topic {get; set;}
        public List<AdditionalField> AdditionalFields {get; set;}
    }
}