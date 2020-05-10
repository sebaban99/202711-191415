using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic
{
    public interface IRequestLogic : ILogic<Request>
    {
        Request GetByCondition(Expression<Func<Request, bool>> expression);
        int Create(Request request);
        Request Update(Request request);
        int AssignRequestNumber();
    }
}
