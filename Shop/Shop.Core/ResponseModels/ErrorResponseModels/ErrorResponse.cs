using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.ResponseModels.ErrorResponseModels
{

    public class ErrorResponse
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string TraceId { get; set; }
        public ErrorDescription Errors { get; set; }
    }

    public class ErrorDescription
    {
        public List<string> Body { get; set; }
    }
}
