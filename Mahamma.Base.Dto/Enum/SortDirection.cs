using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.Dto.Enum
{
    public class SortDirection
    {
        #region Property
        public int Id { get; private set; }
        public string Name { get; private set; }
        #endregion

        #region Enum Values
        public static SortDirection Ascending = new(0, nameof(Ascending).ToLowerInvariant());
        public static SortDirection Descending = new(1, nameof(Descending).ToLowerInvariant());
        #endregion

        #region CTRS
        public SortDirection(int id, string name)
        {
            Id = id;
            Name = name;
        }
        #endregion
    }
}
