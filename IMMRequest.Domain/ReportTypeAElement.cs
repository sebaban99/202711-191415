using System;
using System.Collections.Generic;
using System.Linq;

namespace IMMRequest.Domain
{
    public class ReportTypeAElement : ReportElement
    {
        public Status Status { get; set; }
        public List<int> RequestNumbers { get; set; }

        public ReportTypeAElement()
        {
            RequestNumbers = new List<int>();
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                ReportTypeAElement r = (ReportTypeAElement)obj;
                return this.Status == r.Status && this.RequestNumbers.SequenceEqual(r.RequestNumbers)
                    && this.Amount == r.Amount;
            }
        }
    }
}
