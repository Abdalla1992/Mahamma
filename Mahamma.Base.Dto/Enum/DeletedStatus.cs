using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.Dto.Enum
{
    public class DeletedStatus
    {
        #region Property
        public int Id { get; private set; }
        public string Name { get; private set; }
        #endregion

        #region Enum Values
        public static DeletedStatus NotDeleted = new(1, nameof(NotDeleted).ToLowerInvariant());
        public static DeletedStatus Deleted = new(2, nameof(Deleted).ToLowerInvariant());
        public static DeletedStatus Archived = new(3, nameof(Archived).ToLowerInvariant());
        #endregion

        #region CTRS
        public DeletedStatus(int id, string name)
        {
            Id = id;
            Name = name;
        }
        #endregion
    }
}
