using IMMRequest.Domain;
using System;
using System.Data.Common;
using System.Linq;
using IMMRequest.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IMMRequest.DataAccess
{
    public class RangeRepository : BaseRepository<AFRangeItem>
    {
        public RangeRepository(DbContext context)
        {
            Context = context;
        }

        public override AFRangeItem Get(Guid id)
        {
            try
            {
                return Context.Set<AFRangeItem>().First(x => x.Id == id);
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
