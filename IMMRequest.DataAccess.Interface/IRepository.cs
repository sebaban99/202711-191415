using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IMMRequest.DataAccess.Interfaces
{
    public interface IRepository<T>
    {

        void Add(T entity);

        void Remove(T entity);

        void Update(T entity);

        IEnumerable<T> GetAll();

        T GetByCondition(Expression<Func<T, bool>> expression);

        IEnumerable<T> GetAllByCondition(Expression<Func<T, bool>> expression);

        T Get(Guid id);

        void SaveChanges();
    }
}
