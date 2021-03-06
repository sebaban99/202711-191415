using System;
using System.Collections.Generic;

namespace IMMRequest.Domain
{
    public class AFValue
    {
        public Guid Id { get; set; }
        public virtual Request Request {get; set;}
        public virtual AdditionalField AdditionalField {get; set;}
        public Guid AdditionalFieldID { get; set; }
        public List<AFValueItem> Values {get; set;}
    }
}