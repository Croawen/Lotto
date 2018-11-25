using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lotto.Api.Filters
{
    public class ErrorsResponse
    {
        public string Message { get; set; }
        public IEnumerable<ErrorResponse> Errors { get; set; }
    }
}
