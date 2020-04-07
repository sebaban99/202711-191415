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
    }
}
