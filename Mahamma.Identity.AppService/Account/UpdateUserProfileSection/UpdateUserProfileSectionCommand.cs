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

namespace Mahamma.Identity.AppService.Account.UpdateUserProfileSection
{
    public class UpdateUserProfileSectionCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        public List<UserProfileSectionDto> UserProfileSections { get; set; }

        public UpdateUserProfileSectionCommand(List<UserProfileSectionDto> userProfileSections)
        {
            UserProfileSections = userProfileSections;
        }

        #endregion

        #region CTRS

        #endregion
    }
}
