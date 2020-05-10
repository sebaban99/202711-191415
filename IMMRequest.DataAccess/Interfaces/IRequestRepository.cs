using IMMRequest.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMMRequest.DataAccess
{
    public interface IRequestRepository : IRepository<Request>
    {
        int GetAmountOfElements();
    }
}
