using System;

namespace IMMRequest.DataAccess
{
    public class ExceptionRepository : Exception
    {
        public ExceptionRepository(string message): base(message){ }
        public ExceptionRepository(string message, Exception inner) : base(message, inner) { }  
    }
}