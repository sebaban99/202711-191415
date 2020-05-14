using System;
using System.Collections.Generic;
using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic.Interfaces
{
    public interface ILogLogic
    {
        List<Log> GetLogsByDate(DateTime from, DateTime until); 
        void Add(Log log);
    }
}