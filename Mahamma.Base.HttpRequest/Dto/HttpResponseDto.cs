using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.HttpRequest.Dto
{
    public class HttpResponseDto
    {
        public System.Net.HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; }
    }
}
