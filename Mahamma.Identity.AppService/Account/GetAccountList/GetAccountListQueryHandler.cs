using Mahamma.ApiClient.Dto.Company;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.User.Dto;
using Mahamma.Identity.Domain.User.Repository;
using Mahamma.Identity.Domain.UserRole.Repository;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.GetAccountList
{
    public class GetAccountListQueryHandler : IRequestHandler<GetAccountListQuery, ValidateableResponse<ApiResponse<List<MemberDto>>>>
    {
        #region Props
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        #endregion

        #region CTRS
        public GetAccountListQueryHandler(IUserRepository userRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<List<MemberDto>>>> Handle(GetAccountListQuery request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<List<MemberDto>>> response = new(new ApiResponse<List<MemberDto>>());

            var memberDto = await _userRepository.GetUserList(request.IdList);

            if (memberDto != null)
            {                
                response.Result.CommandMessage = $"Process completed successfully.";
                response.Result.ResponseData = memberDto;
            }
            else
            {
                response.Result.CommandMessage = $"No date found.";
            }
            return response;
        }
    }
}
