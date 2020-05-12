using System;
using System.Data.Common;
using System.Linq;
using IMMRequest.Domain;
using IMMRequest.DataAccess.Interfaces;
using IMMRequest.Exceptions;

namespace IMMRequest.DataAccess
{
    public class SessionRepository : BaseRepository<Session>, ISessionRepository
    {
        public SessionRepository(IMMRequestContext context)
        {
            Context = context;
        }

        public override Session Get(Guid id)
        {
            try
            {
                return Context.Sessions.First(x => x.Id == id);
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
                if (adminId == null)
                {
                    return false;
                }

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