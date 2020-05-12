using IMMRequest.Domain;
using System;
using IMMRequest.Exceptions;
using System.Data.Common;
using System.Linq;

namespace IMMRequest.DataAccess
{
    public class AdminRepository : BaseRepository<Admin>
    {
        public AdminRepository(IMMRequestContext context)
        {
            Context = context;
        }

        public override Admin Get(Guid id)
        {
            try
            {
                return Context.Administrators.First(x => x.Id == id);
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