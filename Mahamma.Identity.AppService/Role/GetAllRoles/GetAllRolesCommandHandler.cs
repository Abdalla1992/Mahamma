using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.Role.Dto;
using Mahamma.Identity.Domain.Role.Repository;
using Mahamma.Identity.Domain.User.Dto;
using Mahamma.Identity.Domain.User.Entity;
using Mahamma.Identity.Domain.User.Repository;
using Mahamma.Identity.Domain.UserRole.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Role.GetAllRoles
{
    public class GetAllRolesCommand : IRequest<ApiResponse<List<RoleDto>>>
    {
    }
    class GetAllRolesCommandHandler : IRequestHandler<GetAllRolesCommand, ApiResponse<List<RoleDto>>>
    {
        #region Prop
        private readonly IRoleRepository _roleRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;        
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        #endregion

        #region Ctor
        public GetAllRolesCommandHandler(IRoleRepository roleRepository,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IUserRepository userRepository,
            IUserRoleRepository userRoleRepository)
        {
            _roleRepository = roleRepository;
            _httpContext = httpContext;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }
        #endregion

        #region Method
        public async Task<ApiResponse<List<RoleDto>>> Handle(GetAllRolesCommand request, CancellationToken cancellationToken)
        {
            ApiResponse<List<RoleDto>> response = new ApiResponse<List<RoleDto>>();
            long currentUserId = (long)_httpContext.HttpContext.Items["UserId"];
            UserDto userDto = await _userRepository.GetById(currentUserId);
            if (userDto != null)
            {
                List<RoleDto> roleList = await _roleRepository.GetAllCompanyRoles((int)userDto.CompanyId);
                if (roleList?.Count > 0)
                {
                    foreach (var role in roleList)
                    {
                        role.Users = new List<UserDto>();
                        List<Domain.UserRole.Entity.UserRole> userRoles = await _userRoleRepository.GetUsersByRoleId(role.Id);
                        if (userRoles?.Count > default(int))
                        {
                            foreach (var userRole in userRoles)
                            {
                                role.Users.Add(await _userRepository.GetById(userRole.UserId));
                            }
                        }
                    }
                    response.ResponseData = roleList;
                    response.CommandMessage = "Role Saved Succefuly";
                }
                else
                {
                    response.CommandMessage = "Failed to add the new Role. Try again shortly.";
                }
            }
            else
            {
                response.CommandMessage = "Failed to add the new Role. Try again shortly.";
            }
            return response;
        }
        #endregion
    }
}
