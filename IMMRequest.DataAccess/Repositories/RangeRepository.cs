using IMMRequest.Domain;
using System;
using System.Data.Common;
using System.Linq;
using IMMRequest.Exceptions;

namespace IMMRequest.DataAccess
{
    public class RangeRepository : BaseRepository<Range>
    {
        public RangeRepository(IMMRequestContext context)
        {
            Context = context;
        }

        public override Range Get(Guid id)
        {
            try
            {
                return Context.RangeValues.First(x => x.Id == id);
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
