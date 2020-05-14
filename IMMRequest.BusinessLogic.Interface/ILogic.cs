using System;
using System.Collections.Generic;

namespace IMMRequest.BusinessLogic.Interfaces
{
    public interface ILogic<T>
    {
        IEnumerable<T> GetAll();
        T Get(Guid id);
    }
}
