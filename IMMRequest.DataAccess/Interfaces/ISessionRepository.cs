using IMMRequest.Domain;

namespace IMMRequest.DataAccess
{
    public interface ISessionRepository : IRepository<Session>
    {
        bool ValidToken(string token);
    }
}