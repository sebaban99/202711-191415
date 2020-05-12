using IMMRequest.Domain;
using System;
using System.Data.Common;
using System.Linq;
using IMMRequest.Exceptions;

namespace IMMRequest.DataAccess
{
    public class AreaRepository : BaseRepository<Area>
    {
        public AreaRepository(IMMRequestContext context)
        {
            Context = context;
        }

        public override Area Get(Guid id)
        {
            try
            {
                return Context.Areas.First(x => x.Id == id);
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

