using System;
using System.Collections.Generic;
using IMMRequest.Domain;
using IMMRequest.DataAccess.Interfaces;
using System.Linq;
using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.Exceptions;

namespace IMMRequest.BusinessLogic
{
    public class LogLogic : ILogLogic
    {
        private const string ALREADY_EXISTS_LOG_MESSAGE = "Log already exists";
        private ILogRepository logRepository;

        public LogLogic(ILogRepository logRepository)
        {
            this.logRepository = logRepository;
        }

         public List<Log> GetAll()
        {
            return logRepository.GetAll().ToList();
        }

        public List<Log> GetLogsByDate(DateTime from, DateTime until)
        {
            return logRepository.GetLogsByDate(from, until);
        }

        public void Add(Log log)
        {
            if (log != null)
            {
                logRepository.Add(log);
                logRepository.SaveChanges();
            }
            else
            {
                throw new BusinessLogicException(ALREADY_EXISTS_LOG_MESSAGE);
            }
        }

        public void Remove(Log log)
        {
            Log logById = Get(log.Id); 
            if (logById != null)
            {
                logRepository.Remove(log);
                logRepository.SaveChanges();
            }
            else
            {
                throw new BusinessLogicException("Log to remove was not found");
            }
        }

        private Log Get(Guid id)
        {
            return logRepository.Get(id);
        }
    }
}