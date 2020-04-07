using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using IMMRequest.Domain;
using System.Linq;
using System.Data.Common;

namespace IMMRequest.DataAccess
{
    public class RequestRepository
    {
        private IMMRequestContext context;
        public RequestRepository(IMMRequestContext context)
        {
            this.context = context;
        }

        public void Add(Request entity)
        {
            try
            {
                context.Set<Request>().Add(entity);
                context.SaveChanges();
            }
            catch (DbException)
            {
                throw new DataAccessException("Error: Could not add entity to DB");
            }
        }

        public void Remove(Request entity)
        {
            try
            {
                context.Set<Request>().Remove(entity);
                context.SaveChanges();
            }
            catch (DbException)
            {
                throw new DataAccessException("Error: Entity could not be removed from DB");

            }
        }

        public Request Get(Guid id)
        {
            try
            {
                return context.Requests.First(x => x.Id == id);
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

        public IEnumerable<Request> GetAll()
        {
            try
            {
                return context.Set<Request>().ToList();
            }
            catch (DbException)
            {
                throw new DataAccessException("Error: could not get Table's elements");
            }
        }

        public void Empty()
        {
            try
            {
                foreach (Request entity in context.Set<Request>())
                {
                    context.Set<Request>().Attach(entity);
                    context.Set<Request>().Remove(entity);
                }
                context.SaveChanges();
            }
            catch (DbException)
            {
                throw new DataAccessException("Error: Table could not be emptied");
            }
        }
    }
}
