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

        public Topic()
        {
            Types = new List<Type>();
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Topic t = (Topic)obj;
                return this.Name == t.Name && this.Area.Equals(t.Area);
            }
        }
    }
}