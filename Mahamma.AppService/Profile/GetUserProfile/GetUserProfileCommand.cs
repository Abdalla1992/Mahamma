using Mahamma.AppService.Profile.Dto;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.MemberSearch.Dto;
using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mahamma.AppService.GetUserProfile.GetUserProfileCommand
{
    public class GetUserProfileCommand : IRequest<ApiResponse<UserProfileDto>>
    {
        #region Prop
        [DataMember]
        public long Id { get; set; }
        #endregion
        public GetUserProfileCommand(int id)
        {
            Id = id;
        }
    }
}
