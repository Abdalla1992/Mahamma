using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System.Runtime.Serialization;

namespace Mahamma.Identity.AppService.UserRole.AuthorizeUser
{
    public class AuthorizeUserCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        [DataMember]
        public long UserId { get; set; }
        [DataMember]
        public int PermissionId { get; set; }
        [DataMember]
        public int PageId { get; set; }
        #endregion

        public AuthorizeUserCommand(long userId, int permissionId, int pageId)
        {
            UserId = userId;
            PermissionId = permissionId;
            PageId = pageId;
        }
    }
}
