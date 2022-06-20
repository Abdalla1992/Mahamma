using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.CompleteUserProfile
{
   public class CompleteUserProfileCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        [DataMember]
        public string ProfileImage { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string JobTitle { get; set; }
        [DataMember]
        public int WorkingDays { get; set; }
        [DataMember]
        public int WorkingHours { get; set; }
        [DataMember]
        public string InvitationId { get; set; }
        #endregion
    }
}
