using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.Role.Dto;
using Mahamma.Identity.Domain.Role.Repository;
using Mahamma.Identity.Domain.User.Dto;
using Mahamma.Identity.Domain.User.Repository;
using Mahamma.Identity.Domain.UserRole.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Role.GetRole
{
    class GetRoleCommandHandler : IRequestHandler<GetRoleCommand, ValidateableResponse<ApiResponse<RoleDto>>>
    {
        #region Prop
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        #endregion

        #region Ctor
        public GetRoleCommandHandler(IRoleRepository roleRepository, IUserRepository userRepository,
            IUserRoleRepository userRoleRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }
        #endregion

        #region Method
        public async Task<ValidateableResponse<ApiResponse<RoleDto>>> Handle(GetRoleCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<RoleDto>> response = new(new ApiResponse<RoleDto>());
            RoleDto roleDto = await _roleRepository.GetRole(request.Id);
            if (roleDto != null)
            {
                roleDto.Users = new List<UserDto>();
                List<Domain.UserRole.Entity.UserRole> userRoles = await _userRoleRepository.GetUsersByRoleId(roleDto.Id);
                if (userRoles?.Count > default(int))
                {
                    foreach (var userRole in userRoles)
                    {
                        roleDto.Users.Add(await _userRepository.GetById(userRole.UserId));
                    }
                }
                response.Result.CommandMessage = "Process completed successfully.";
                response.Result.ResponseData = roleDto;
            }
            else
            {
                response.Result.CommandMessage = "No data found.";
            }
            return response;
        }
        #endregion
    }
}
