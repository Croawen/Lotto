using System;
using System.Collections.Generic;
using System.Text;

namespace Lotto.Common.Exceptions
{
    public class TransactionRepositoryException : ApiException
    {
        public override int StatusCode => 500;

        public TransactionRepositoryException(string message) : base(message) { }
    }
}
