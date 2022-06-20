using Mahamma.Base.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Task.Enum
{
    public class TaskAcceptedRejectedStatus
    {
        #region Property
        public int Id { get; private set; }
        public string Name { get; private set; }
        #endregion

        #region Enum Values
        public static TaskAcceptedRejectedStatus NotReviewed = new(1, nameof(NotReviewed));
        public static TaskAcceptedRejectedStatus Accepted = new(2, nameof(Accepted));
        public static TaskAcceptedRejectedStatus Rejected = new(3, nameof(Rejected));
        public static TaskAcceptedRejectedStatus AcceptedAfterRejected = new(4, nameof(AcceptedAfterRejected));
        #endregion

        #region CTRS
        public TaskAcceptedRejectedStatus(int id, string name)
        {
            Id = id;
            Name = name;
        }
        #endregion

    }
}

