using IMMRequest.Domain;

namespace IMMRequest.DataAccess.Interfaces
{
    public interface IRequestRepository : IRepository<Request>
    {
        int GetAmountOfElements();
    }
}
