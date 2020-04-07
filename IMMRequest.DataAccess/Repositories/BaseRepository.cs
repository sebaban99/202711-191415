using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace IMMRequest.DataAccess
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected IMMRequestContext Context { get; set; }

        public void Add(T entity)
        {
            try
            {
                Context.Set<T>().Add(entity);
                Context.SaveChanges();
            }
            catch (DbException)
            {
                throw new DataAccessException("Error: Could not add entity to DB");
            }
        }

        public void Remove(T entity)
        {
            try
            {
                Context.Set<T>().Remove(entity);
                Context.SaveChanges();
            }
            catch (DbException)
            {
                throw new DataAccessException("Error: Entity could not be removed from DB");

            }
        }

        public void Update(T entity)
        {
            try
            {
                Context.Entry(entity).State = EntityState.Modified;
                Context.Set<T>().Update(entity);
                Context.SaveChanges();
            }
            catch (DbException)
            {
                throw new DataAccessException("Error: Could not update Entity in DB");
            }
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                return Context.Set<T>().ToList();
            }
            catch (DbException)
            {
                throw new DataAccessException("Error: could not get Table's elements");
            }
        }

        public abstract T Get(Guid id);

        public T GetByCondition(Expression<Func<T, bool>> expression)
        {
            try
            {
                return Context.Set<T>().First(expression);
            }
            catch (DbException)
            {
                throw new DataAccessException("Error: could not retrieve Entity");
            }
        }

        public void Save()
        {
            try
            {
                this.Context.SaveChanges();
            }
            catch (DbException)
            {
                throw new DataAccessException("Error: changes could not be applied to DB");
            }
        }

        public void Empty()
        {
            try
            {
                foreach (T entity in Context.Set<T>())
                {
                    Context.Set<T>().Attach(entity);
                    Context.Set<T>().Remove(entity);
                }
                Context.SaveChanges();
            }
            catch (DbException)
            {
                throw new DataAccessException("Error: Table could not be emptied");
            }
        }

    }
}