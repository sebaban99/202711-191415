using System;
using System.Collections.Generic;
using System.Text;

namespace IMMRequest.Importer
{
    public class TypeImpModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public TopicImpModel Topic { get; set; }
        public bool IsActive { get; set; }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                TypeImpModel t = (TypeImpModel)obj;
                return this.Name == t.Name && this.Topic.Equals(t.Topic);
            }
        }
    }
}
