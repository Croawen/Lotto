using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lotto.Api.Filters
{
    public class ErrorResponse
    {
        public string Name { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
