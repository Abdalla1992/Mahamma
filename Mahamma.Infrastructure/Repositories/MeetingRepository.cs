
using AutoMapper;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.Enum;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Meeting.Repositroy;
using Mahamma.Domain.Meeting.Dto;
using Mahamma.Domain.Meeting.Entity;
using Mahamma.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Repositories
{
    public class MeetingRepository : Base.EntityRepository<Meeting>, IMeetingRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public MeetingRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }

        public async Task<PageList<MeetingDto>> GetMeetings(SearchMeetingDto searchMeetingDto, string role, string superAdminRole, long currentUserId, int companyId)
        {
            #region Declare Return Var with Intial Value
            PageList<MeetingDto> meetingListDto = new();
            #endregion
            #region Preparing Filter 
            Expression<Func<Meeting, bool>> filter;
            if (role == superAdminRole)
            {
                filter = t => t.DeletedStatus == DeletedStatus.NotDeleted.Id
                                                         && (t.CompanyId == companyId)
                                                         && (searchMeetingDto.Filter.WorkSpaceId == default || t.WorkSpaceId == searchMeetingDto.Filter.WorkSpaceId)
                                                         && (searchMeetingDto.Filter.ProjectId == default || t.ProjectId == searchMeetingDto.Filter.ProjectId)
                                                         && (searchMeetingDto.Filter.TaskId == default || t.TaskId == searchMeetingDto.Filter.TaskId)
                                                         && (string.IsNullOrEmpty(searchMeetingDto.Filter.Title) || t.Title.ToLower().Contains(searchMeetingDto.Filter.CleanName));
            }
            else
            {
                filter = t => t.DeletedStatus == DeletedStatus.NotDeleted.Id
                                                         && (t.CompanyId == companyId)
                                                         && (searchMeetingDto.Filter.WorkSpaceId == default || t.WorkSpaceId == searchMeetingDto.Filter.WorkSpaceId)
                                                         && (searchMeetingDto.Filter.ProjectId == default || t.ProjectId == searchMeetingDto.Filter.ProjectId)
                                                         && (searchMeetingDto.Filter.TaskId == default || t.TaskId == searchMeetingDto.Filter.TaskId)
                                                         && (string.IsNullOrEmpty(searchMeetingDto.Filter.Title) || t.Title.ToLower().Contains(searchMeetingDto.Filter.CleanName));
            }
            #endregion

            List<Meeting> meetingList = searchMeetingDto.Sorting.Column switch
            {
                "name" => await GetPageAsyncWithoutQueryFilter(searchMeetingDto.Paginator.Page, searchMeetingDto.Paginator.PageSize, filter, x => x.Title, searchMeetingDto.Sorting.SortingDirection.Id),
                _ => await GetPageAsyncWithoutQueryFilter(searchMeetingDto.Paginator.Page, searchMeetingDto.Paginator.PageSize, filter, x => x.Id, SortDirection.Descending.Id),
            };
            if (meetingList?.Count > default(int))
            {
                int totalCount = await GetCountAsyncWithoutQueryFilter(filter);
                meetingListDto.SetResult(totalCount, Mapper.Map<List<Meeting>, List<MeetingDto>>(meetingList));
            }
            return meetingListDto;
        }
        public void AddMeeting(Meeting meeting)
        {
            CreateAsyn(meeting);
        }
        public async Task<MeetingDto> GetDtoById(int id, long userId, string currentUserRole, string superAdminRoleName, int companyId)
        {
            if (currentUserRole == superAdminRoleName)
            {
                return Mapper.Map<MeetingDto>(await FirstOrDefaultNoTrackingAsync(m => m.Id == id
                    && m.CompanyId == companyId));
            }
            else
            {
                return Mapper.Map<MeetingDto>(await FirstOrDefaultNoTrackingAsync(m => m.Id == id
                    && m.Members.Any(m => m.UserId == userId && m.DeletedStatus == DeletedStatus.NotDeleted.Id)));
            }
        }
        public async Task<Meeting> GetById(int id)
        {
            return await FirstOrDefaultAsync(ll => ll.Id == id);
        }
        public void UpdateMeeting(Meeting meeting)
        {
            Update(meeting);
        }
        public async Task<List<MeetingDto>> GetMeetingListByProjectId(int projectId)
        {
            List<Meeting> meetingslist = await GetWhereAsync(p => p.ProjectId == projectId);
            return Mapper.Map<List<MeetingDto>>(meetingslist);
        }

        public async Task<List<long>> GetMembersUserIdList(int meetingId)
        {
            return (await FirstOrDefaultAsync(t => t.Id == meetingId, "Members")).Members.Select(m => m.UserId).ToList();
        }

        public async Task<List<MinuteOfMeetingDto>> GetMinutesOfMeeting(int meetingId)
        {
            return Mapper.Map<List<MinuteOfMeetingDto>>((await FirstOrDefaultAsync(t => t.Id == meetingId, "MinutesOfMeeting")).MinutesOfMeeting.ToList());
        }

        public async Task<Meeting> getmeeting(int minofmeet)
        {
            return await FirstOrDefaultAsync(t => t.MinutesOfMeeting.Any(t => t.Id == minofmeet));
        }

        public async Task<bool> ValidDate(int meetingId)
        {
           var meeting =await  GetById(meetingId);
            if (meeting ==null)
            {
                return false;
            }
            return meeting.Date > DateTime.Now ;
        }

        public async Task<List<MinuteOfMeetingDto>> GetMinuteOfMeetingByTaskId(int taskId)
        {
            var meeting =await GetWhereAsync(m => m.MinutesOfMeeting.Any(s => s.TaskId == taskId), "MinutesOfMeeting");
            var minuteOfMeeting = meeting.SelectMany(m => m.MinutesOfMeeting).Where(w =>w.TaskId == taskId).ToList();
            return Mapper.Map<List<MinuteOfMeetingDto>>(minuteOfMeeting);
        }
        public async Task<List<MinuteOfMeetingDto>> GetMinuteOfMeetingByProjectId(int projectId)
        {
            var meeting = await GetWhereAsync(m => m.MinutesOfMeeting.Any(s => s.ProjectId == projectId), "MinutesOfMeeting");
            var minuteOfMeeting = meeting.SelectMany(m => m.MinutesOfMeeting).Where(w => w.ProjectId == projectId).ToList();
            return Mapper.Map<List<MinuteOfMeetingDto>>(minuteOfMeeting);
        }

        public async Task<DateTime?> GetTaskUpComingMeetingDate(int taskId)
        {
            var meeting = (await GetWhereAsync(m => m.TaskId == taskId)).OrderByDescending(m => m.Date).FirstOrDefault();

            return meeting?.Date;
        }

        public async Task<DateTime?> GetProjectUpComingMeetingDate(int projectId)
        {
            var meeting = (await GetWhereAsync(m => m.ProjectId == projectId)).OrderByDescending(m => m.Date).FirstOrDefault();

            return meeting?.Date;
        }
    }
}
