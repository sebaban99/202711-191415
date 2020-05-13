using IMMRequest.Domain;
using IMMRequest.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;
using System.Linq;

namespace IMMRequest.DataAccess
{
    public class AdditionalFieldRepository : BaseRepository<AdditionalField>
    {
        public AdditionalFieldRepository(DbContext context)
        {
            Context = context;
        }

        public override AdditionalField Get(Guid id)
        {
            try
            {
                return Context.Set<AdditionalField>().First(x => x.Id == id);
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