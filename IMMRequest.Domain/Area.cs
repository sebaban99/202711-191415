using System;
using System.Collections.Generic;

namespace IMMRequest.Domain
{
    public class Area
    {
        public Guid Id {get; set;}
        public string Name {get; set;}
        public List<Topic> Topics {get; set;}
    }
}