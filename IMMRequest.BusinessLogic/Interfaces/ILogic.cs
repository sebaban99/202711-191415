using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace IMMRequest.BusinessLogic
{
    public interface ILogic<T>
    {
        IEnumerable<T> GetAll();
        T Get(Guid id);
    }
}
