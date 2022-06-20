using AutoMapper;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.Meeting.Dto;
using Mahamma.Domain.Meeting.Entity;
using Mahamma.Domain.MemberSearch.Dto;
using Mahamma.Domain.MyWork.Dto;
using Mahamma.Domain.MyWork.Entity;
using Mahamma.Domain.Project.Dto;
using Mahamma.Domain.Project.Entity;
using Mahamma.Domain.ProjectAttachment.Dto;
using Mahamma.Domain.ProjectAttachment.Entity;
using Mahamma.Domain.Task.Dto;
using Mahamma.Domain.Task.Entity;
using Mahamma.Domain.TaskActivity.Dto;
using Mahamma.Domain.TaskActivity.Entity;
using System.Linq;

namespace Mahamma.Infrastructure.AutoMapper
{
    public class MahammaProfile : Profile
    {
        public MahammaProfile()
        {
            CompanyMapping();
            WorkspaceMapping();
            ProjectAttachmentMapping();
            TaskMapping();
            TaskLogMapping();
            ProjectMapping();
            ProjectActivitiesMapping();
            CompanyInvitationMapping();
            TaskCommentMapping();
            MeetingMapping();
            ProjectCommentMapping();
            ProjectCharterMapping();
            ProjectRiskPlanMapping();
            ProjectCommunicationPlanMapping();
            FolderMapping();
            FolderFileMapping();
            NoteMapping();
        }

      

        private void TaskLogMapping()
        {
            CreateMap<TaskActivity, TaskActivityDto>();
        }

        private void CompanyMapping()
        {
            CreateMap<Domain.Company.Entity.Company, Domain.Company.Dto.CompanyDto>();
        }

        private void WorkspaceMapping()
        {
            CreateMap<Domain.Workspace.Entity.Workspace, Domain.Workspace.Dto.WorkspaceDto>();
        }
        private void TaskMapping()
        {
            CreateMap<TaskMember, TaskMemberDto>();

            CreateMap<TaskComment, TaskCommentDto>()
                .BeforeMap((src, dest) => dest.LikesCount = src.Likes?.Count ?? 0)
                .ForMember(dto => dto.Replies, act => act.MapFrom(src => src.Replies.OrderByDescending(c => c.CreationDate)));

            CreateMap<Domain.Task.Entity.Task, TaskDto>()
                .ForMember(dto => dto.TaskAttachments, act => act.MapFrom(src => src.Attachments))
                .ForMember(dto => dto.SubTasks, act => act.MapFrom(src => src.SubTask))
                .ForMember(dto => dto.TaskMembers, act => act.MapFrom(src => src.TaskMembers))
                .ForMember(dto => dto.ProjectName, act => act.MapFrom(src => src.Project.Name))
                .ForMember(dto => dto.TaskComments, act
                    => act.MapFrom(src => src.TaskMembers.SelectMany(m => m.TaskComments.Where(c => c.ParentCommentId == null).OrderByDescending(c => c.CreationDate))));

            CreateMap<Domain.Task.Entity.Task, DropDownItem<int>>();
        }
        private void ProjectAttachmentMapping()
        {
            CreateMap<ProjectAttachment, ProjectAttachmentDto>();
        }

        private void ProjectMapping()
        {
            CreateMap<Domain.Project.Entity.Project, Domain.Project.Dto.ProjectDto>()
                .ForMember(dto => dto.ProjectComments, act
                 => act.MapFrom(src => src.ProjectMembers.SelectMany(m => m.ProjectComments.Where(c => c.ParentCommentId == null).OrderByDescending(c => c.CreationDate))));

            CreateMap<Domain.Project.Entity.Project, Domain.Project.Dto.ProjectUserDto>();

            CreateMap<Domain.Project.Entity.ProjectMember, Domain.Project.Dto.ProjectMemberDto>();

            CreateMap<ProjectComment, ProjectCommentDto>()
            .BeforeMap((src, dest) => dest.LikesCount = src.Likes?.Count ?? 0)
            .ForMember(dto => dto.Replies, act => act.MapFrom(src => src.Replies.OrderByDescending(c => c.CreationDate)));

            CreateMap<Domain.Project.Entity.Project, Domain.Project.Dto.ProjectDto>();


        }
        private void ProjectActivitiesMapping()
        {
            CreateMap<Domain.ProjectActivity.Entity.ProjectActivity, Domain.ProjectActivity.Dto.ProjectActivityDto>();
        }
        private void CompanyInvitationMapping()
        {
            CreateMap<Domain.Company.Entity.CompanyInvitation, Domain.Company.Dto.CompanyInvitationDto>();
        }
        private void TaskCommentMapping()
        {
            CreateMap<TaskComment, TaskCommentDto>()
                .BeforeMap((src, dest) => dest.LikesCount = src.Likes?.Count ?? 0)
                .ForMember(dto => dto.Replies, act => act.MapFrom(src => src.Replies.OrderByDescending(c => c.CreationDate)));
        }

        private void ProjectCommentMapping()
        {
            CreateMap<ProjectComment, ProjectCommentDto>()
             .BeforeMap((src, dest) => dest.LikesCount = src.Likes?.Count ?? 0)
             .ForMember(dto => dto.Replies, act => act.MapFrom(src => src.Replies.OrderByDescending(c => c.CreationDate)));
        }

        private void MeetingMapping()
        {
            CreateMap<Meeting, MeetingDto>()
                .BeforeMap((src, dest) => 
                dest.MemberList = src.Members.Where(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id).ToDictionary(
                    keySelector: m => m.UserId,
                    elementSelector: m => m.MeetingRoles.Where(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id).Select(mr => mr.MeetingRoleId).ToList()))
                .BeforeMap((src, dest) => dest.AttendanceIdList = src.Members.Where(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id && (a.Attended ?? false)).Select(m => m.UserId).ToList())
                .BeforeMap((src, dest) => src.AgendaTopics.Where(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id).ToList())
                   .BeforeMap((src, dest) => src.MeetingFiles.Where(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id).ToList())
                .ForMember(src => src.Members, opt => opt.Ignore())
                .BeforeMap((src, dest) => dest.Members = new());

            CreateMap<MeetingAgendaTopics, AgendaTopicDto>();
            CreateMap<MeetingFile, MeetingFilesDto>();
            CreateMap<MinuteOfMeeting, MinuteOfMeetingDto>();
        }
        private void ProjectCharterMapping()
        {
            CreateMap<ProjectCharter, ProjectCharterDto>().ReverseMap();
        }
        private void ProjectRiskPlanMapping()
        {
            CreateMap<ProjectRiskPlan, ProjectRiskPlanDto>().ReverseMap();
        }
        private void ProjectCommunicationPlanMapping()
        {
            CreateMap<ProjectCommunicationPlan, ProjectCommunicationPlanDto>().ReverseMap();
        }
        private void FolderMapping()
        {
            CreateMap<Folder, FolderDto>();
        }
        private void FolderFileMapping()
        {
            CreateMap<FolderFile, FolderFileDto>();
        }

        private void NoteMapping()
        {
            CreateMap<Note, NoteDto>().ReverseMap();
        }

    }
}
