using IMMRequest.Domain;
using System;
using IMMRequest.Exceptions;
using System.Data.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace IMMRequest.DataAccess
{
    public class AdminRepository : BaseRepository<Admin>
    {
        public AdminRepository(DbContext context)
        {
            Context = context;
        }

        public override Admin Get(Guid id)
        {
            try
            {
                return Context.Set<Admin>().First(x => x.Id == id);
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