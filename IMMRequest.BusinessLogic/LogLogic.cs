using System;
using System.Collections.Generic;
using IMMRequest.Domain;
using IMMRequest.DataAccess;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace IMMRequest.BusinessLogic

{
    public class LogLogic : ILogLogic
    {
        private const string Message1 = "Ya existe un Log con ese id";
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
            if (this.Get(log.Id.ToString()) != null)
            {
                logRepository.Add(log);
            }
            else
            {
                throw new Exception(Message1.ToString());
            }
        }

        public void Remove(Log log)
        {
            if (this.Get(log.Id.ToString()) != null)
            {
                logRepository.Remove(log);
            }
        }

        private Log Get(string id)
        {
            Guid logId = new Guid(id);
            return logRepository.Get(logId);
        }

    }
}