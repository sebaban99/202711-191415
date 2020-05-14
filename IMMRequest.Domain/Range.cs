using System;

namespace IMMRequest.Domain
{
    public class Range
    {
        public Guid Id {get; set;}  
        public AdditionalField AdditionalField { get; set; }
        public string Value {get; set;}
    }
}