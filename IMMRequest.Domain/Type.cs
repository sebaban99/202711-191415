using System;
using System.Collections.Generic;

namespace IMMRequest.Domain
{
    public class Type
    {
        public Guid Id {get; set;}
        public string Name {get; set;}
        public DateTime CreationDate { get; set; }
        public Topic Topic {get; set;}
        public bool IsActive { get; set; }
        public List<AdditionalField> AdditionalFields {get; set;}

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Type t = (Type)obj;
                return this.Name == t.Name && this.Topic.Equals(t.Topic);
            }
        }
    }
}