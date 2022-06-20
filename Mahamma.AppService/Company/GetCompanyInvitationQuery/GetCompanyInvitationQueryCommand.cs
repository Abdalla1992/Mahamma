using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.Company.Dto;
using MediatR;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Company.GetCompanyInvitationQuery
{
    public class GetCompanyInvitationQueryCommand : IRequest<ValidateableResponse<ApiResponse<CompanyInvitationDto>>>
    {
        [DataMember]
        public string InvitationId { get; set; }

        public GetCompanyInvitationQueryCommand(string invitationId)
        {
            InvitationId = invitationId;
        }
    }
}
