using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.DataAccess.Interfaces
{
    public interface ITypeRepository : IRepository<Type>
    {
        void SoftDelete(Type type);

        IEnumerable<Type> GetActiveTypes();
    }
}
