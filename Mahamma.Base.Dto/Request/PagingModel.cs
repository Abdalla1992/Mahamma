using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.Dto.Request
{
    public class PagingModel
    {
        private int _Page;
        /// <summary>
        /// The list view number.
        /// </summary>
        public int Page
        {
            get
            {
                return _Page;
            }
            set
            {
                if (value > 0)
                    value -= 1;

                _Page = value;
            }
        }
        /// <summary>
        /// The list view size.
        /// </summary>
        public int PageSize { get; set; }
    }
}
