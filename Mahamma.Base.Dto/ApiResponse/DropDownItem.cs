using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.Dto.ApiResponse
{
    public class DropDownItem<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
    }
}
