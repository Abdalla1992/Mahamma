using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.Role.Repository;
using Mahamma.Identity.Domain.UserRole.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.UserRole.AuthorizeUser
{
    public class AuthorizeUserCommandHandler : IRequestHandler<AuthorizeUserCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRolePagePermissionRepository _rolePermissionRepository;
        private readonly IPagePermissionRepository _pagePermissionRepository;
        #endregion

        #region ctor
        public AuthorizeUserCommandHandler(IUserRoleRepository userRoleRepository, IRolePagePermissionRepository rolePermissionRepository,
            IPagePermissionRepository pagePermissionRepository)
        {
            _userRoleRepository = userRoleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _pagePermissionRepository = pagePermissionRepository;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(AuthorizeUserCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            long roleId = await _userRoleRepository.GetRoleIdByUserId(request.UserId);
            if (roleId > default(long))
            {
                int pagePermissionId = await _pagePermissionRepository.GetIdByPageIdAndPermissionId(request.PageId, request.PermissionId);
                if (pagePermissionId > default(int))
                {
                    bool authorized = await _rolePermissionRepository.CheckRolePermission(roleId, pagePermissionId);
                    if (authorized)
                    {
                        response.Result.ResponseData = authorized;
                        response.Result.CommandMessage = "Authorized";
                    }
                    else
                    {
                        response.Result.CommandMessage = "UnAuthorized";
                    }
                }
                else
                {
                    response.Result.CommandMessage = "UnAuthorized";
                }
            }
            else
            {
                response.Result.CommandMessage = "Unauthorizable";
            }
            return response;
        }
    }
}
