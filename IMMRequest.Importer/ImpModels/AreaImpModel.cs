using System;
using System.Collections.Generic;
using System.Text;

namespace IMMRequest.Importer
{
    public class AreaImpModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<TopicImpModel> Topics { get; set; }

        public AreaImpModel()
        {
            Topics = new List<TopicImpModel>();
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                AreaImpModel a = (AreaImpModel)obj;
                return this.Name == a.Name;
            }
        }
    }
}
