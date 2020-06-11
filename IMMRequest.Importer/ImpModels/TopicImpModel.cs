using System;
using System.Collections.Generic;
using System.Text;

namespace IMMRequest.Importer
{
    public class TopicImpModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AreaImpModel Area { get; set; }
        public List<TypeImpModel> Types { get; set; }

        public TopicImpModel()
        {
            Types = new List<TypeImpModel>();
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                TopicImpModel t = (TopicImpModel)obj;
                return this.Name == t.Name && this.Area.Equals(t.Area);
            }
        }
    }
}
