using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.Resources.IResourceReader
{
    public interface IMessageResourceReader
    {

        public string GetKeyValue(string key, int languageId);

    }
}
