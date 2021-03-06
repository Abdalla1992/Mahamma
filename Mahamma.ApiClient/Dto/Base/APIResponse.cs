using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.ApiClient.Dto.Base
{
    public class APIResponse<T>
    {
        public Result<T> Result { get; set; }
        public bool IsValidResponse { get; set; }
        public List<object> Errors { get; set; }
    }

    public class Result<T>
    {
        public T ResponseData { get; set; }

        public string CommandMessage { get; set; }
    }
}
