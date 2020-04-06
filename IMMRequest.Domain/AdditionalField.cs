using System;
using System.Collections.Generic;

namespace IMMRequest.Domain
{
    public class AdditionalField
    {
        public Guid Id {get; set;}
        public string Name {get; set;}
        public Type Type {get; set;}
        public List<Range> Range {get; set;}
        public string Value {get; set;}
    }
}