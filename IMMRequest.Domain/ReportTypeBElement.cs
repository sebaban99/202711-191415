using System;

namespace IMMRequest.Domain
{
    public class ReportTypeBElement : ReportElement
    {
        public Type Type { get; set; }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                ReportTypeBElement r = (ReportTypeBElement)obj;
                return this.Type == r.Type && this.Amount == r.Amount;
            }
        }
    }
}