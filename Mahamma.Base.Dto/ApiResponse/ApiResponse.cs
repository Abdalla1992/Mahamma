using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.Dto.ApiResponse
{
    public class ApiResponse<T>
    {
        public T ResponseData { get; set; }
        public string CommandMessage { get; set; }
    }
}
