using System;
using IMMRequest.Domain;

namespace IMMRequest.DataAccess
{
    public interface ISessionRepository : IRepository<Session>
    {
        bool ValidateSession(Guid token);
    }
}