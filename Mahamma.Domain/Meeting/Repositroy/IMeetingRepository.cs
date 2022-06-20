using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Meeting.Dto;
using Mahamma.Domain.Meeting.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mahamma.Domain.Meeting.Repositroy
{
    public interface IMeetingRepository : IRepository<Entity.Meeting>
    {
        void AddMeeting(Entity.Meeting meeting);
        Task<PageList<MeetingDto>> GetMeetings(SearchMeetingDto searchMeetingDto, string role, string superAdminRole, long currentUserId, int companyId);
        Task<MeetingDto> GetDtoById(int id, long userId, string currentUserRole, string superAdminRoleName, int companyId);
        Task<Entity.Meeting> GetById(int id);
        void UpdateMeeting(Entity.Meeting meeting);
        Task<bool> ValidDate(int meetingId);
        Task<List<MeetingDto>> GetMeetingListByProjectId(int projectId);
        Task<List<long>> GetMembersUserIdList(int id);
        Task<List<MinuteOfMeetingDto>> GetMinutesOfMeeting(int meetingId);
        Task<Entity.Meeting> getmeeting(int minofmeet);
        Task<List<MinuteOfMeetingDto>> GetMinuteOfMeetingByTaskId(int taskId);
        Task<List<MinuteOfMeetingDto>> GetMinuteOfMeetingByProjectId(int projectId);
        Task<DateTime?> GetTaskUpComingMeetingDate(int taskId);
        Task<DateTime?> GetProjectUpComingMeetingDate(int projectId);
    }
}
