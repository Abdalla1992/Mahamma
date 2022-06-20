using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.PagePermissionByRoleId.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.PagePermissionByRole.GetPagePermissionByRole
{
    public class GetPagePermissionByRoleCommand : IRequest<ValidateableResponse<ApiResponse<List<PagePermissionLocalizationDto>>>>
    {
        //[DataMember]
        //public long currentUserId { get; set; }
        [DataMember]
        public long currentRoleId { get; set; }
    }
}
