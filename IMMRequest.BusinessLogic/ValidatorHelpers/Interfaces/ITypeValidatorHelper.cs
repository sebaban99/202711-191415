using System;
using System.Collections.Generic;
using System.Text;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.BusinessLogic
{
    public interface ITypeValidatorHelper
    {
        void ValidateAdd(Type type);

        void ValidateDelete(Type type);
    }
}
