using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Workspace.AssignMember
{
   public class AssignWorkSpaceMemberCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        [DataMember]
        public int WorkSpaceId { get; set; }
        [DataMember]
        public List<long> UserIds { get; set; }


        #endregion
    }
}
