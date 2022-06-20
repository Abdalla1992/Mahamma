using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.MemberSearch.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.MemberSearch.SearchUserForTask
{
    public class SearchUserForTaskCommand : IRequest<ValidateableResponse<ApiResponse<List<MemberDto>>>>
    {
        #region Prop
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int TaskId { get; set; }
        #endregion
    }
}
