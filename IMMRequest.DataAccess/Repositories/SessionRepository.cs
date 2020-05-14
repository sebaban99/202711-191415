using System;
using System.Data.Common;
using System.Linq;
using IMMRequest.Domain;
using IMMRequest.DataAccess.Interfaces;
using IMMRequest.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IMMRequest.DataAccess
{
    public class SessionRepository : BaseRepository<Session>, ISessionRepository
    {
        public SessionRepository(DbContext context)
        {
            Context = context;
        }

        public override Session Get(Guid id)
        {
            try
            {
                return Context.Set<Session>().First(x => x.Id == id);
            }
            catch (System.InvalidOperationException)
            {
                return null;
            }
            catch (DbException)
            {
                throw new DataAccessException("Error: could not get specific Admin");
            }
        }

        public bool ValidateSession(Guid adminId)
        {
            try
            {
                Session session = GetByCondition(q => q.AdminId == adminId);

                return session != null;
            }
            catch (Exception e)
            {
                throw (new DataAccessException("Error, the session is not valid " + e));
            }
        }
    }
}