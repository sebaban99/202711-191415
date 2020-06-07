using System;

namespace IMMRequest.Domain
{
    public class AFRangeItem
    {
        public Guid Id {get; set;}  
        public AdditionalField AdditionalField { get; set; }
        public string Value {get; set;}
    }
}