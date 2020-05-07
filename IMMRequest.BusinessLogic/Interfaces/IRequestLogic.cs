using System;
using System.Collections.Generic;
using System.Text;
using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic
{
    public interface IRequestLogic : ILogic<Request>
    {
        int Create(Request request);
        Request Update(Request request);
    }
}
