using System;
using System.Data.Common;
using System.Linq;
using IMMRequest.DataAccess.Repositories;
using IMMRequest.Domain;

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

        public bool ValidToken(string token)
        {
            try
            {
                Guid validToken;

                if (string.IsNullOrEmpty(token))
                {
                    return false;
                }
                bool isValid = Guid.TryParse(token, out validToken);
                Guid receivedToken = new Guid(token);

                Guid query = (from q in Context.Administrators
                                 where q.SessionToken == receivedToken
                                 select q).FirstOrDefault().Id;

                if (query != null)
                {
                    return isValid && true;
                }
                return isValid && false;
            }
            catch (NullReferenceException eNull)
            {
                throw (new Exception("Security error: the token doesn't exist, " + eNull));
            }
            catch (Exception e)
            {
                throw (new Exception("Error, the token is not valid " + e));
            }
        }
    }
}