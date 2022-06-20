using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.Resources.Dto
{
    public class ResourceFileData
    {
        public string Key { get; set; }
        public Dictionary<string, string> LocalizedValue { get; set; }
    }
}
