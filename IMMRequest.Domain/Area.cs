using System;
using System.Collections.Generic;

namespace IMMRequest.Domain
{
    public class Area
    {
        public Guid Id {get; set;}
        public string Name {get; set;}
        public List<Topic> Topics {get; set;}

        public Area()
        {
            Topics = new List<Topic>();
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Area a = (Area)obj;
                return this.Name == a.Name;
            }
        }
    }
}