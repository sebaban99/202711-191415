using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using IMMRequest.Domain;

namespace IMMRequest.WebApi
{
    [ExcludeFromCodeCoverage]
    public class LogDTO
    {
 
        public Guid Id {get;set;}
 
        public string ActionType { get; set; }
 
        public DateTime Date { get; set; }
 
        public Admin Admin { get; set; }
 
        public string Email { get; set; }

        public LogDTO(Log log){
            this.Id = log.Id;
            this.ActionType = log.ActionType;
            this.Date = log.Date;
            this.Admin = log.Admin;
            this.Email = log.Email;
            
        }
        public LogDTO(){}

    }
}