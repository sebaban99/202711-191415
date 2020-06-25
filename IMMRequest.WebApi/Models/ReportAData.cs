using IMMRequest.Domain;
using System;

namespace IMMRequest.WebApi
{
    public class ReportAData
    {
        public string From { get; set; }
        public string Until { get; set; }
        public string Email { get; set; }

        public ReportAData() { }
    }
}
