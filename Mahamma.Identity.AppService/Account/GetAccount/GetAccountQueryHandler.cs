using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.Role.Repository;
using Mahamma.Identity.Domain.User.Dto;
using Mahamma.Identity.Domain.User.Entity;
using Mahamma.Identity.Domain.User.Repository;
using Mahamma.Identity.Domain.UserRole.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.GetAccount
{
    public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, ValidateableResponse<ApiResponse<UserDto>>>
    {
        #region Props
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        public readonly UserManager<User> _userManager;
        #endregion

        #region CTRS
        public GetAccountQueryHandler(IUserRepository userRepository, IUserRoleRepository userRoleRepository, IRoleRepository roleRepository, 
            UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<UserDto>>> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            Log.Warning($"Get account query handler");
            ValidateableResponse<ApiResponse<UserDto>> response = new(new ApiResponse<UserDto>());

            UserDto userDto = await _userRepository.GetById(request.Id);

            if (userDto != null)
            {
                response.Result.CommandMessage = $"Process completed successfully.";
                if (userDto.UserProfileSections?.Count > default(int))
                    userDto.UserProfileSections = userDto.UserProfileSections.OrderBy(x => x.OrderId).ToList();
                response.Result.ResponseData = userDto;
                var user = _userManager.Users.Where(u => u.Id == request.Id).FirstOrDefault();
                response.Result.ResponseData.RoleId = await _userRoleRepository.GetRoleIdByUserId(request.Id);
                if (response.Result.ResponseData.RoleId > 0)
                    response.Result.ResponseData.RoleName = (await _roleRepository.GetRoleById(response.Result.ResponseData.RoleId))?.Name;
            }
            else
            {
                response.Result.CommandMessage = $"No date found.";
            }
            return response;
        }
    }
}
