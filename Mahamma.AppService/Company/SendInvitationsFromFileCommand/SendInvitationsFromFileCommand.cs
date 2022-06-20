using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.Company.Dto;
using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Company.SendInvitationsFromFileCommand
{
    public class SendInvitationsFromFileCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {

        #region Prop
        [DataMember]
        public List<InvitationFileDto> UploadedFiles { get; set; }
        #endregion
    }
}
