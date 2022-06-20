using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.UpdateUserProfileStatus
{
    public class UpdateUserProfileStatusCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        [DataMember]
        public int UserProfileStatusId { get; set; }
    }
}
