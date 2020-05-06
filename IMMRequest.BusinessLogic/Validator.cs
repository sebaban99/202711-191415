using System;
using System.Collections.Generic;
using System.Text;

namespace IMMRequest.BusinessLogic
{
    public abstract class Validator<T>
    {
        public abstract void ValidateAdd(T entity);

        public abstract void ValidateDelete(T entity);

        public abstract void ValidateUpdate(T entity);
    }
}
