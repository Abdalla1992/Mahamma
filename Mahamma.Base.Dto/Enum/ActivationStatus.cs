using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.Dto.Enum
{
    public class ActivationStatus
    {
        #region Property
        public int Id { get; private set; }
        public string Name { get; private set; }
        #endregion

        #region Enum Values
        public static ActivationStatus Active = new(1, nameof(Active).ToLowerInvariant());
        public static ActivationStatus InActive = new(2, nameof(InActive).ToLowerInvariant());
        #endregion

        #region CTRS
        public ActivationStatus(int id, string name)
        {
            Id = id;
            Name = name;
        }
        #endregion
    }
}
