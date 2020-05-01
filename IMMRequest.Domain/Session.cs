using System;

namespace IMMRequest.Domain
{
    public class Session
    {
        public Guid Id { get; set; }

        public static Admin LoggedAdmin {get;set;}
    }
}
