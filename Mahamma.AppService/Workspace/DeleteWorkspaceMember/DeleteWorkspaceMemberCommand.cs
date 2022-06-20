using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Workspace.DeleteWorkspaceMember
{
    public class DeleteWorkspaceMemberCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        [DataMember]
        public long UserId { get; set; }
        [DataMember]
        public int WorkspaceId { get; set; }

        public DeleteWorkspaceMemberCommand(long userId, int workspaceId)
        {
            UserId = userId;
            WorkspaceId = workspaceId;
        }
    }
}
