using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.HttpRequest.Settings
{
    internal class HttpRequestSettings
    {
        public int RequestTimeout { get; set; } = 30000;
        public bool IgnoreSSL { get; set; } = true;
    }
}
