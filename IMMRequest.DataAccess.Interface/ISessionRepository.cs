using System;
using IMMRequest.Domain;

namespace IMMRequest.DataAccess.Interfaces
{
    public interface ISessionRepository : IRepository<Session>
    {
        bool ValidateSession(Guid token);
    }
}