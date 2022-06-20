using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.MemberSearch.Dto;
using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mahamma.AppService.MemberSearch.SearchUserForWorkspace
{
    public class SearchUserForWorkspaceCommand : IRequest<ValidateableResponse<ApiResponse<List<MemberDto>>>>
    {
        #region Prop
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int WorkspaceId { get; set; }
        #endregion
    }
}
