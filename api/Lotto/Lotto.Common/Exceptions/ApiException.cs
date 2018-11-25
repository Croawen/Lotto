using System;
using System.Collections.Generic;
using System.Text;

namespace Lotto.Common.Exceptions
{
    public abstract class ApiException : Exception
    {
        public abstract int StatusCode { get; }

        protected ApiException(string message) : base(message) { }

        public ApiException(string message, Exception innerException) : base(message, innerException) { }
    }
}
