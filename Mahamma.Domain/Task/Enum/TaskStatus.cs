using Mahamma.Base.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mahamma.Domain.Task.Enum
{
    public class TaskStatus : Enumeration
    {
        #region Enum Values
        public static TaskStatus New = new(1, "New");
        public static TaskStatus InProgress = new(2, "InProgress");
        public static TaskStatus InProgressWithDelay = new(3, "InProgressWithDelay");
        public static TaskStatus CompletedEarly = new(4, "CompletedEarly");
        public static TaskStatus CompletedOnTime = new(5, "CompletedOnTime");
        public static TaskStatus CompletedLate = new(6, "CompletedLate");
        #endregion
        #region CTRS
        public TaskStatus(int id, string name)
            : base(id, name)
        {
        }
        #endregion

        #region Methods
        public static IEnumerable<TaskStatus> List() => new[] { New, CompletedOnTime, CompletedLate, CompletedEarly, InProgress, InProgressWithDelay};
        public static IEnumerable<TaskStatus> NotCompletedList() => new[] { New, InProgress, InProgressWithDelay };
        public static IEnumerable<TaskStatus> CompletedList() => new[] { CompletedEarly, CompletedOnTime, CompletedLate};
        public static TaskStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for TaskStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static TaskStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new Exception($"Possible values for TaskStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        #endregion
    }
}
