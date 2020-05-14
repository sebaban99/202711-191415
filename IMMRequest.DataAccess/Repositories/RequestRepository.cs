using System;
using IMMRequest.Exceptions;
using IMMRequest.Domain;
using IMMRequest.DataAccess.Interfaces;
using System.Linq;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace IMMRequest.DataAccess
{
    public class RequestRepository : BaseRepository<Request>, IRequestRepository
    {
        public RequestRepository(DbContext context)
        {
            this.Context = context;
        }

        public override Request Get(Guid id)
        {
            try
            {
                return Context.Set<Request>().First(x => x.Id == id);
            }
            catch (System.InvalidOperationException)
            {
                return null;
            }
            catch (DbException)
            {
                throw new DataAccessException("Error: could not retrieve specific Entity");
            }
        }

        public int GetAmountOfElements()
        {
            try
            {
                return Context.Set<Request>().Count();
            }
            catch (DbException)
            {
                throw new DataAccessException("Error: could not retrieve amount of Requests in DB");
            }
        }
    }
}
