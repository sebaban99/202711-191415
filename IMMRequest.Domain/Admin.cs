﻿using System;

namespace IMMRequest.Domain
{
    public class Admin
    {
        public Guid Id {get; set;}
        public string Name {get; set;}
        public string Email {get; set;}
        public string Password {get; set;}

        public Admin ()
        {
            this.Name = string.Empty;
            this.Email = string.Empty;
            this.Password = string.Empty;

        }
    }

}
