using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Identity.Domain.Role.Dto;
using Mahamma.Identity.Domain.Role.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Role.ListRole
{
    public class ListRoleQueryHandler : IRequestHandler<SearchRoleDto, PageList<RoleDto>>
    {
        #region MyRegion
        private readonly IRoleRepository _roleRepository;
        #endregion

        #region Ctor
        public ListRoleQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        #endregion
        public Task<PageList<RoleDto>> Handle(SearchRoleDto request, CancellationToken cancellationToken)
        {
            return _roleRepository.GetRoleData(request);
        }
    }
}
