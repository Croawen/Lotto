using System;
using System.Collections.Generic;
using System.Text;

namespace Lotto.Common.Exceptions
{
    public class BadRequestException : ApiException
    {
        public override int StatusCode => 400;

        public BadRequestException(string message) : base(message) { }
    }
}
