using System;
using System.Diagnostics.CodeAnalysis;

namespace IMMRequest.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException() { }
        public BusinessLogicException(string message) : base(message) { }
        public BusinessLogicException(string message, Exception inner) : base(message, inner) { }
    }
}