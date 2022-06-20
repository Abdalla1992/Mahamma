using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.User.Dto;
using MediatR;
using System.Runtime.Serialization;

namespace Mahamma.Identity.AppService.Account.GetAccount
{
    public class GetAccountQuery : IRequest<ValidateableResponse<ApiResponse<UserDto>>>
    {
        #region Props
        [DataMember]
        public long Id { get; set; }
        #endregion

        #region CTRS
        public GetAccountQuery(int id)
        {
            Id = id;
        }
        #endregion
    }
}
