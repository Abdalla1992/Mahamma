using Mahamma.Base.Dto.Enum;

namespace Mahamma.Domain.Meeting.Enum
{
    public class MeetingRole : Enumeration
    {
        #region Enum Values
        public static MeetingRole Creator = new(1, nameof(Creator).ToLowerInvariant());
        public static MeetingRole Presenter = new(2, nameof(Presenter).ToLowerInvariant());
        public static MeetingRole Speaker = new(3, nameof(Speaker).ToLowerInvariant());
        public static MeetingRole MinuteOfMeetingWriter = new(4, nameof(MinuteOfMeetingWriter).ToLowerInvariant());
        #endregion

        #region CTRS
        public MeetingRole(int id, string name) : base(id, name)
        {
        }
        #endregion
    }
}
