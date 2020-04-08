using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using IMMRequest.Domain;
using System.Linq;
using System.Data.Common;
using System.Linq.Expressions;

namespace IMMRequest.DataAccess
{
    public class RequestRepository : BaseRepository<Request>
    {
        public RequestRepository(IMMRequestContext context)
        {
            this.Context = context;
        }

        public override Request Get(Guid id)
        {
            try
            {
                return Context.Requests.First(x => x.Id == id);
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
    }
}
