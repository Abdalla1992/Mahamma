using Mahamma.Base.Dto.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.Dto.Dto
{
    public class BaseDto<T>:CleanObj
    {
        public T Id { get; set; }
    }
}
