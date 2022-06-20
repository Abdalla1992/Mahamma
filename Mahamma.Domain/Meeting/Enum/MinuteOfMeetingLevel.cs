using Mahamma.Base.Dto.Enum;

namespace Mahamma.Domain.Meeting.Enum
{
    public class MinuteOfMeetingLevel : Enumeration
    {
        #region Enum Values
        public static MinuteOfMeetingLevel NoAction = new(1, nameof(NoAction).ToLowerInvariant());
        public static MinuteOfMeetingLevel NewProject = new(2, nameof(NewProject).ToLowerInvariant());
        public static MinuteOfMeetingLevel ExistingProject = new(3, nameof(ExistingProject).ToLowerInvariant());
        public static MinuteOfMeetingLevel NewTask = new(4, nameof(NewTask).ToLowerInvariant());
        public static MinuteOfMeetingLevel ExistingTask = new(5, nameof(ExistingTask).ToLowerInvariant());
        public static MinuteOfMeetingLevel NewSubTask = new(6, nameof(NewSubTask).ToLowerInvariant());
        public static MinuteOfMeetingLevel ExistingSubTask = new(7, nameof(ExistingSubTask).ToLowerInvariant());
        #endregion

        #region CTRS
        public MinuteOfMeetingLevel(int id, string name) : base(id, name)
        {
        }
        #endregion
    }
}
