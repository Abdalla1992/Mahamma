using Mahamma.Base.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mahamma.Domain.Task.Enum
{
    public class TaskPriority : Enumeration
    {
        #region Enum Values
        public static TaskPriority Normal = new(1, nameof(Normal));
        public static TaskPriority Major = new(2, nameof(Major));
        public static TaskPriority Urgent = new(3, nameof(Urgent));
        #endregion
        #region CTRS
        public TaskPriority(int id, string name)
            : base(id, name)
        {
        }
        #endregion

        #region Methods
        public static IEnumerable<TaskPriority> List() => new[] { Normal, Major, Urgent };
        public static TaskPriority FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for TaskPriority: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static TaskPriority From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new Exception($"Possible values for TaskPriority: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        #endregion
    }
}
