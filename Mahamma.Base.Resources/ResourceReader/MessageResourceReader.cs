using Mahamma.Base.Resources.Enum;
using Mahamma.Base.Resources.IResourceReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.Resources.ResourceReader
{
    public class MessageResourceReader :  Base.BaseFileReader, IMessageResourceReader
    {
        #region CTRS
        public MessageResourceReader() : base(LocalizationType.Message)
        { }

      
        #endregion







    }
}
