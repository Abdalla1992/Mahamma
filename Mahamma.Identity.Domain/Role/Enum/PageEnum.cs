using Mahamma.Base.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.Role.Enum
{
    public class PageEnum : Enumeration
    {
        #region Enum Values
        public static PageEnum WorkspaceProfile = new(1, nameof(WorkspaceProfile));
        public static PageEnum ProjectProfile = new(2, nameof(ProjectProfile));
        public static PageEnum TaskProfile = new(3, nameof(TaskProfile));
        public static PageEnum SubtaskProfile = new(4, nameof(SubtaskProfile));
        public static PageEnum ManageRoles = new(5, nameof(ManageRoles));
        //public static PageEnum DashboardProfile = new(6, nameof(DashboardProfile));
        public static PageEnum MeetingProfile = new(7, nameof(MeetingProfile));
        public static PageEnum ManageMeetings = new(8, nameof(ManageMeetings));
        //public static PageEnum ManageMinutesOfMeetings = new(9, nameof(ManageMinutesOfMeetings));
        #endregion
        #region CTRS
        public PageEnum(int id, string name)
            : base(id, name)
        {
        }
        #endregion

        #region Methods
        public static IEnumerable<PageEnum> List() => new[] { WorkspaceProfile, ProjectProfile, TaskProfile, SubtaskProfile, ManageRoles,
             MeetingProfile, ManageMeetings };//, ManageMinutesOfMeetings }; DashboardProfile
        public static PageEnum FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for PageEnum: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static PageEnum From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new Exception($"Possible values for PageEnum: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        #endregion
    }
}
