using Mahamma.ApiClient.Dto.Company;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.User.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.GetAccountList
{
    public class GetAccountListQuery : IRequest<ValidateableResponse<ApiResponse<List<MemberDto>>>>
    {
        #region Props
        [DataMember]
        public List<long> IdList { get; set; }
        #endregion
    }
}
