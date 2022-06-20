using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.UpdateUserProfileSetting
{
    public class UpdateUserProfileSettingCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        [DataMember]
        public string ProfileImage { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string JobTitle { get; set; }
        [DataMember]
        public string NewPassword { get; set; }
        [DataMember]
        public string CurrentPassword { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public int LanguageId { get; set; }
        [DataMember]
        public string Bio { get; set; }
        [DataMember]
        public List<string> Skills { get; set; }
    }
}
