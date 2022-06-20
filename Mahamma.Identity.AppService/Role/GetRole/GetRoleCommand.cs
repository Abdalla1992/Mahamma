using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.Role.Dto;
using MediatR;
using System.Runtime.Serialization;

namespace Mahamma.Identity.AppService.Role.GetRole
{
    public class GetRoleCommand : IRequest<ValidateableResponse<ApiResponse<RoleDto>>>
    {
        #region Prop
        [DataMember]
        public int Id { get; set; }
        #endregion
        public GetRoleCommand(int id)
        {
            Id = id;
        }
    }
}
