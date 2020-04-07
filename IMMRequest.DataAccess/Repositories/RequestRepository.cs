using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace IMMRequest.DataAccess
{
    public class RequestRepository<Request> : GenericRepository<Request> where Request : class
    {
        public Request GetByRequestNumber (int requestNumber)
        {
            throw new NotImplementedException();
        }
    }
}
