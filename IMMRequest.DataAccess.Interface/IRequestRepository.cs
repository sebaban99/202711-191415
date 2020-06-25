using IMMRequest.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IMMRequest.DataAccess.Interfaces
{
    public interface IRequestRepository : IRepository<Request>
    {
        int GetAmountOfElements();
    }
}
