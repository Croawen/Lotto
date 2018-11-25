using System;
using System.Collections.Generic;
using System.Text;

namespace Lotto.Common.Exceptions
{
    public class NotFoundException : ApiException
    {
        public override int StatusCode => 404;

        public NotFoundException(string message) : base(message) { }
    }
}
