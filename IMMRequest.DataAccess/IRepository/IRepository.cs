using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IMMRequest.DataAccess
{
    public interface IRepository<T> where T:class
    {
        
        void Add(T entidad);

        void Remove(T elem);

        void Update(T entidad);

        IEnumerable<T> GetAll();

        T Get(Guid id);

        void Save();
        
    }
}
