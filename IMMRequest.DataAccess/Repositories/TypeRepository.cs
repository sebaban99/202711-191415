using IMMRequest.Domain;
using System;
using System.Data.Common;
using System.Linq;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.DataAccess.Repositories
{
    public class TypeRepository : BaseRepository<Type>
    {
        public TypeRepository(IMMRequestContext context)
        {
            Context = context;
        }

        public override Type Get(Guid id)
        {
            try
            {
                return Context.Types.First(x => x.Id == id);
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