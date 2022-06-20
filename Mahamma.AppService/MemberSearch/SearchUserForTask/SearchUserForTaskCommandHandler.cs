using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.MemberSearch.Dto;
using Mahamma.Domain.Task.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.MemberSearch.SearchUserForTask
{
    public class SearchUserForTaskCommandHandler : IRequestHandler<SearchUserForTaskCommand, ValidateableResponse<ApiResponse<List<MemberDto>>>>
    {
        #region Props
        private readonly IMemberSearchReadRepository _memberSearchReadRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;

        #endregion

        #region CTRS
        public SearchUserForTaskCommandHandler(IMemberSearchReadRepository memberSearchReadRepository, IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _memberSearchReadRepository = memberSearchReadRepository;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;

        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<List<MemberDto>>>> Handle(SearchUserForTaskCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<List<MemberDto>>> response = new(new ApiResponse<List<MemberDto>>());
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];

            var result = await _memberSearchReadRepository.SearchForMemerToAssignToTask(request.Name, currentUser.CompanyId, currentUser.Id, request.TaskId);
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
