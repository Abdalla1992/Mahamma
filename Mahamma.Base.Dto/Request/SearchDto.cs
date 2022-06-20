using Mahamma.Base.Dto.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.Dto.Request
{
    public class SearchDto<T> : CleanObj where T : class
    {
        public T Filter { get; set; }
        public PagingModel Paginator { get; set; }
        public SortingModel Sorting { get; set; }
    }
}
