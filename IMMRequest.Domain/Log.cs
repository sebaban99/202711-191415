using System;
using System.Text;
using System.Collections.Generic;

namespace IMMRequest.Domain
{
    public class Log
    {
        public enum actionTypeAvailable { login, generateRequest };

        public Guid Id { get; set; }
        public string ActionType { get; set; }
        public DateTime Date { get; set; }
        public Admin Admin { get; set; }
        public string Email { get; set; }

        public Log()
        {
            this.Id = Guid.NewGuid();
            this.ActionType = actionTypeAvailable.login.ToString();
            this.Date = DateTime.Now;
            this.Admin = null;
            this.Email = string.Empty;
        }
    }
}