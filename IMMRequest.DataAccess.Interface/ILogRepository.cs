using System.Collections.Generic;
using IMMRequest.Domain;
using System;
namespace IMMRequest.DataAccess.Interfaces
{
    public interface ILogRepository : IRepository<Log>
    {
        List<Log> GetLogsByDate(DateTime from, DateTime until); 
    }
}