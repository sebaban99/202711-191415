using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace IMMRequest.BusinessLogic
{
    public interface ILogic<T>
    {
        void Create(T entity);

        void Remove(T entity);

        void Update(T entity);

        IEnumerable<T> GetAll();

        T GetByCondition(Expression<Func<T, bool>> expression);

        T Get(string id);
    }
}
