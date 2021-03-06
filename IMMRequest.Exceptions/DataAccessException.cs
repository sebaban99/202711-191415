using System;
using System.Diagnostics.CodeAnalysis;

namespace IMMRequest.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class DataAccessException : Exception
    {
        public DataAccessException() { }
        public DataAccessException(string message): base(message){ }
        public DataAccessException(string message, Exception inner) : base(message, inner) { }  
    }
}