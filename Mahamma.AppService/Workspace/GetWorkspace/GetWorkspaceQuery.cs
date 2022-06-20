using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.Workspace.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Workspace.GetWorkspace
{
    public class GetWorkspaceQuery : IRequest<ValidateableResponse<ApiResponse<WorkspaceUserDto>>>
    {
        #region Props
        [DataMember]
        public int Id { get; set; }
        #endregion

        #region CTRS
        public GetWorkspaceQuery(int id)
        {
            Id = id;
        }
        #endregion
    }
}
