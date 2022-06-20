using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.UserRole.GetUserPermission
{
    public class GetUserPermissionQuery : IRequest<ValidateableResponse<ApiResponse<List<int>>>>
    {
        [DataMember]
        public int PageId { get; set; }
    }
}
