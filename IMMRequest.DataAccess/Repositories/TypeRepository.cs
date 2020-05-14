using IMMRequest.Domain;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using IMMRequest.DataAccess.Interfaces;
using IMMRequest.Exceptions;
using Type = IMMRequest.Domain.Type;
using Microsoft.EntityFrameworkCore;

namespace IMMRequest.DataAccess
{
    public class TypeRepository : BaseRepository<Type>, ITypeRepository
    {
        public TypeRepository(DbContext context)
        {
            Context = context;
        }

        public override Type Get(Guid id)
        {
            try
            {
                return Context.Set<Type>().First(x => x.Id == id);
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
            return Context.Set<Type>().Where(t => t.IsActive == true);
        }

        public void SoftDelete(Type type)
        {
            type.IsActive = false;
            Update(type);
        }
    }
}