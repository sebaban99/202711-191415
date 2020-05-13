using IMMRequest.Domain;
using System;
using IMMRequest.Exceptions;
using System.Data.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace IMMRequest.DataAccess
{
    public class AFValueRepository : BaseRepository<AFValue>
    {
        public AFValueRepository(DbContext context)
        {
            Context = context;
        }

        public override AFValue Get(Guid id)
        {
            try
            {
                return Context.Set<AFValue>().First(x => x.Id == id);
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
    }
}

