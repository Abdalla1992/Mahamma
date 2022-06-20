using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region 
    //Assembly IPMATS.Core.IResources, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// F:\Mahma Records\IPMATS.Core.IResources\bin\Debug\netstandard2.1\IPMATS.Core.IResources.dll
#endregion


namespace Mahamma.Base.Resources.IResourceReader
{
    public interface INotificationResourceReader
    {
        public string GetKeyValue(string key, int languageId);

    }
}
