using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace IMMRequest.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ImportException : Exception
    {
        public ImportException() { }
        public ImportException(string message) : base(message) { }
        public ImportException(string message, Exception inner) : base(message, inner) { }
    }
}
