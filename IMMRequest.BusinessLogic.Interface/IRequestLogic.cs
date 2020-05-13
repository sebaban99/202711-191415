using System;
using System.Linq.Expressions;
using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic.Interfaces
{
    public interface IRequestLogic : ILogic<Request>
    {
        Request GetByCondition(Expression<Func<Request, bool>> expression);
        int Create(Request request);
        Request Update(Request request);
        int AssignRequestNumber();
    }
}
