using System;
using System.Text;
using System.Collections.Generic;

namespace IMMRequest.Domain
{
    public class Log
    {
        public Guid Id { get; set; }
        public enum actionTypeAvailable { login, generateRequest };
        public string ActionType { get; set; }
        public DateTime Date { get; set; }
        public Admin Admin { get; set; }
        public string Email { get; set; }

        public Log()
        {
           
            this.ActionType = actionTypeAvailable.login.ToString();
            this.Date = DateTime.Now;
            this.Admin = null;
            this.Email = string.Empty;
        }

        public Log(string type, Admin a)
        {
            this.ActionType = type;
            this.Date = DateTime.Now;
            this.Admin = a;
            this.Email = a.Email;
        }
    }
}