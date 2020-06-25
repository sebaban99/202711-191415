using System;
using System.Collections.Generic;
using System.Text;

namespace IMMRequest.Domain
{
    public class AFValueItem
    {
        public Guid Id { get; set; }
        public AFValue AFValue { get; set; }
        public string Value { get; set; }
    }
}
