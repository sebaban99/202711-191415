using IMMRequest.Domain;
using System;
using System.Data.Common;
using System.Linq;

namespace IMMRequest.DataAccess.Repositories
{
    public class AdditionalFieldRepository : BaseRepository<AdditionalField>
    {
        public AdditionalFieldRepository(IMMRequestContext context)
        {
            Context = context;
        }

        public override AdditionalField Get(Guid id)
        {
            try
            {
                return Context.AdditionalFields.First(x => x.Id == id);
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