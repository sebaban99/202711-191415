﻿using System;
using System.Collections.Generic;
using System.Text;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.BusinessLogic
{
    public interface ITypeLogic : ILogic<Type>
    {
        Type Create(Type type);
        void Remove(Type type);
    }
}