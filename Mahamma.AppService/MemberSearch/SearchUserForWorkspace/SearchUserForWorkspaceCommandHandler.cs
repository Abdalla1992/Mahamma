﻿using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.MemberSearch.Dto;
using Mahamma.Domain.Task.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.MemberSearch.SearchUserForWorkspace
{
    public class SearchUserForWorkspaceCommandHandler : IRequestHandler<SearchUserForWorkspaceCommand, ValidateableResponse<ApiResponse<List<MemberDto>>>>
    {
        #region Props
        private readonly IMemberSearchReadRepository _memberSearchReadRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;

        #endregion

        #region CTRS
        public SearchUserForWorkspaceCommandHandler(IMemberSearchReadRepository memberSearchReadRepository, IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _memberSearchReadRepository = memberSearchReadRepository;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;

        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<List<MemberDto>>>> Handle(SearchUserForWorkspaceCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<List<MemberDto>>> response = new(new ApiResponse<List<MemberDto>>());
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];

            var result = await _memberSearchReadRepository.SearchForMemerToAssignToWorkspace(request.Name, currentUser.CompanyId, currentUser.Id, request.WorkspaceId);
            if (result?.Count > 0)
            {
                response.Result.ResponseData = result;
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("SearchedSuccessfully",currentUser.LanguageId);
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("NoUsersFound",currentUser.LanguageId);
            }
            return response;
        }
    }
}
