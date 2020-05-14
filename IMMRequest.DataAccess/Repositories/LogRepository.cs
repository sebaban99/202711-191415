using IMMRequest.Domain;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using IMMRequest.Exceptions;
using IMMRequest.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMMRequest.DataAccess
{
    public class LogRepository : BaseRepository<Log>, ILogRepository
    {
        public LogRepository(DbContext context)
        {
            Context = context;
        }

        public override Log Get(Guid id)
        {
            try
            {
                return Context.Set<Log>().FirstOrDefault(x => x.Id == id);
            }
            catch (System.InvalidOperationException)
            {
                return null;
            }
            catch (DbException)
            {
                throw new DataAccessException("Error: could not get specific Entity");
            }
        }

        public List<Log> GetLogsByDate(DateTime from, DateTime until)
        {
            try
            {
                return Context.Set<Log>().ToList().Where(x=>
                {
                    bool retorno=false;
                    if(x.Date.CompareTo(from)>0||x.Date.CompareTo(from)==0)
                    {
                        retorno=true;
                    }
                    if(x.Date.CompareTo(until)<0||x.Date.CompareTo(until)==0)
                    {
                        retorno = retorno && true;
                    }
                    return retorno;
                } ).ToList();
            }
            catch (DbException e)
            {
                throw (new DataAccessException("Error: cannot GetLogsByDate " + e.Message));
            }
        }
    }
}
