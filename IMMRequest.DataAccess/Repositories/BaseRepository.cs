using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using IMMRequest.Exceptions;
using IMMRequest.DataAccess.Interfaces;

namespace IMMRequest.DataAccess
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected DbContext Context { get; set; }

        public void Add(T entity)
        {
            try
            {
                Context.Set<T>().Add(entity);
            }
            catch (DbException)
            {
                throw new DataAccessException("Error: Could not add entity or a component of it to DB");
            }
        }

        public void Remove(T entity)
        {
            try
            {
                Context.Set<T>().Remove(entity);
            }
            catch (DbUpdateException)
            {
                throw new DataAccessException("Error: Entity to remove doesn't exist in the current context");
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
            }
            catch (DbUpdateException)
            {
                throw new DataAccessException("Error: Entity to update doesn't exist in the current context");
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
                return Context.Set<T>().FirstOrDefault(expression);
            }
            catch (DbException)
            {
                throw new DataAccessException("Error: could not retrieve Entity");
            }
        }

        public void SaveChanges()
        {
            try
            {
                this.Context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                throw new DataAccessException("Error: changes could not be applied to DB " + e.Message);
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

        public virtual IEnumerable<T> GetAllByCondition(Expression<Func<T, bool>> expression)
        {
            try
            {
                return Context.Set<T>().Where(expression);
            }
            catch (DbException)
            {
                throw new DataAccessException("Error: could not retrieve Entity");
            }
        }
    }
}