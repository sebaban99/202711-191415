using System;
using System.Collections.Generic;

namespace IMMRequest.Domain
{
    public class Topic
    {
        public Guid Id {get; set;}
        public string Name {get; set;}
        public Area Area {get; set;}
        public List<Type> Types {get; set;}
    }
}