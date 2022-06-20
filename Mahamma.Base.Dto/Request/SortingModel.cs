using Mahamma.Base.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.Dto.Request
{
    public class SortingModel
    {
        /// <summary>
        /// The sorting column name.
        /// </summary>
        public string Column { get; set; }

        private string _Direction;
        public string Direction
        {
            get
            {
                return _Direction;
            }
            set
            {
                _Direction = value;

                if (value == "asc")
                    SortingDirection = SortDirection.Ascending;
                else
                    SortingDirection = SortDirection.Descending;
            }
        }

        /// <summary>
        /// The sorting direction.
        /// <para>Direction should be:</para>
        /// <para> 0 : Ascending</para>
        /// <para> 1 : Descending</para>
        /// </summary>
        public SortDirection SortingDirection { get; set; }
    }
}
