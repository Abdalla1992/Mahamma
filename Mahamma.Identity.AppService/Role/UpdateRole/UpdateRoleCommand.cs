using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Role.UpdateRole
{
   public class UpdateRoleCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<long> UserIds { get; set; }
        [DataMember]
        public List<int> PagePermissionIds { get; set; }
    }
}
