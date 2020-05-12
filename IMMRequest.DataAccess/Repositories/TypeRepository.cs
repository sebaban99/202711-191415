using IMMRequest.Domain;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.DataAccess
{
    public class TypeRepository : BaseRepository<Type>, ITypeRepository
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


        public IEnumerable<Type> GetActiveTypes()
        {
            return Context.Types.Where(t => t.IsActive == true);
        }

        public void SoftDelete(Type type)
        {
            type.IsActive = false;
            Update(type);
        }
    }
}