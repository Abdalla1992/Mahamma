using Mahamma.ApiClient.Dto.Company;
using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Meeting.Repositroy;
using Mahamma.Domain.Project.Dto;
using Mahamma.Domain.Project.Entity;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Domain.ProjectAttachment.Dto;
using Mahamma.Domain.ProjectAttachment.Repository;
using Mahamma.Identity.ApiClient.Dto.Base;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Identity.ApiClient.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.GetProject
{
    public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, ValidateableResponse<ApiResponse<ProjectUserDto>>>
    {
        #region Prop
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectAttachmentRepository _projectAttachmentRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IAccountService _accountService;
        private readonly AppSetting _appSetting;
        private readonly IMeetingRepository _meetingRepository;

        #endregion

        #region Ctor
        public GetProjectQueryHandler(IProjectRepository projectRepository, IProjectMemberRepository projectMemberRepository,
            IHttpContextAccessor httpContext, IAccountService accountService, IProjectAttachmentRepository projectAttachmentRepository,
            AppSetting appSetting,IMeetingRepository meetingRepository)
        {
            _projectRepository = projectRepository;
            _projectMemberRepository = projectMemberRepository;
            _httpContext = httpContext;
            _accountService = accountService;
            _projectAttachmentRepository = projectAttachmentRepository;
            _appSetting = appSetting;
            _meetingRepository = meetingRepository;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<ProjectUserDto>>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<ProjectUserDto>> response = new(new ApiResponse<ProjectUserDto>());
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ProjectDto projectDto = await _projectRepository.GetById(request.Id, currentUser.Id, currentUser.RoleName, _appSetting.SuperAdminRole, currentUser.CompanyId, request.RequestedFromMeeting);
            projectDto.UpComingMeetingDate = await _meetingRepository.GetProjectUpComingMeetingDate(request.Id);
            projectDto.ProjectMinuteOfMeetings = await _meetingRepository.GetMinuteOfMeetingByProjectId(request.Id);
            if (projectDto != null)
            {
                ProjectUserDto projectUserDto = new ProjectUserDto
                {
                    Id = projectDto.Id,
                    Name = projectDto.Name,
                    CreatorUserId = projectDto.CreatorUserId,
                    Description = projectDto.Description,
                    DueDate = projectDto.DueDate,
                    WorkSpaceId = projectDto.WorkSpaceId,
                    Members = new List<Domain.MemberSearch.Dto.MemberDto>(),
                    UserIdList = new List<long>(),
                    ProgressPercentage = projectDto.ProgressPercentage,
                    ProjectMinuteOfMeetings = projectDto.ProjectMinuteOfMeetings,
                    ProjectComments = projectDto.ProjectComments ,
                };
                //projectUserDto.ProjectComments.ForEach(c => c.Member = SetMembers(c.UserId ?? 0).FirstOrDefault());
                
                foreach (var comment in projectUserDto.ProjectComments)
                {
                    comment.Member = SetMembers(comment.UserId ?? 0).FirstOrDefault();
                    comment.IsLikedByCurrentUser = CheckCommentLikedByCurrentUser(comment.Id, currentUser.Id);
                    
                    //comment.Replies.ForEach(c => );
                    foreach (var reply in comment.Replies)
                    {
                        reply.Member = SetMembers(reply.UserId ?? 0).FirstOrDefault();
                        reply.IsLikedByCurrentUser = CheckCommentLikedByCurrentUser(reply.Id, currentUser.Id);
                    }
                }

                List<ProjectMember> projectMembers = await _projectMemberRepository.GetProjectMemberByProjectId(projectDto.Id);
                if (projectMembers?.Count > 0)
                {
                    foreach (var member in projectMembers)
                    {
                        UserDto userDto = _accountService.GetUserById(new BaseRequestDto() { AuthToken = GetAccessToken() }, member.UserId);
                        if (userDto != null)
                        {
                            projectUserDto.Members.Add(new Domain.MemberSearch.Dto.MemberDto
                            {
                                UserId = userDto.Id,
                                FullName = userDto.FullName,
                                ProfileImage = userDto.ProfileImage,
                                Rating = member.Rating
                            });
                            projectUserDto.UserIdList.Add(userDto.Id);
                        }
                    }
                }
                PageList<ProjectAttachmentDto> pageList = await _projectAttachmentRepository.GetProjectLatestFileList(projectDto.Id, 3);
                projectUserDto.ProjectAttachments = pageList.DataList;
                projectUserDto.FilesCount = pageList.TotalCount;
                response.Result.CommandMessage = "Process completed successfully.";
                response.Result.ResponseData = projectUserDto;
            }
            else
            {
                response.Result.CommandMessage = "No data found.";
            }
            return response;
        }

        private List<Domain.MemberSearch.Dto.MemberDto> SetMembers(params long[] userIdList)
        {
            var result = new List<Domain.MemberSearch.Dto.MemberDto>();
            foreach (var userId in userIdList)
            {
                UserDto userDto = _accountService.GetUserById(new BaseRequestDto() { AuthToken = GetAccessToken() }, userId);
                if (userDto != null)
                {
                    result.Add(new Domain.MemberSearch.Dto.MemberDto
                    {
                        UserId = userDto.Id,
                        FullName = userDto.FullName,
                        ProfileImage = userDto.ProfileImage
                    });
                }
            }
            return result;
        }

        private string GetAccessToken()
        {
            string apiToken = string.Empty;
            if (_httpContext.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues value))
                apiToken = value;

            return apiToken;
        }

        private bool CheckCommentLikedByCurrentUser(int commentId, long userId)
        {
            return _projectRepository.CheckCommentLikedByCurrentUser(commentId, userId);
        }
    }
}
