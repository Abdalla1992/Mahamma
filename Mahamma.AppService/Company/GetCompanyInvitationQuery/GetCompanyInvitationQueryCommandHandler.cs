using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Company.Dto;
using Mahamma.Domain.Company.Repositroy;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Company.GetCompanyInvitationQuery
{
    public class GetCompanyInvitationQueryCommandHandler : IRequestHandler<GetCompanyInvitationQueryCommand, ValidateableResponse<ApiResponse<CompanyInvitationDto>>>
    {
        #region Props
        private readonly ICompanyInvitationRepository _companyInvitationRepository;
        private readonly IMessageResourceReader _messageResourceReader;

        #endregion

        #region Ctor
        public GetCompanyInvitationQueryCommandHandler(ICompanyInvitationRepository companyInvitationRepository, IMessageResourceReader messageResourceReader)
        {
            _companyInvitationRepository = companyInvitationRepository;
            _messageResourceReader = messageResourceReader;

        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<CompanyInvitationDto>>> Handle(GetCompanyInvitationQueryCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<CompanyInvitationDto>> response = new(new ApiResponse<CompanyInvitationDto>());
            CompanyInvitationDto companyInvitationDto = await _companyInvitationRepository.GetCompanyInvitationByInvitationId(request.InvitationId);
            if (companyInvitationDto != null)
            {
                response.Result.ResponseData = companyInvitationDto;
            }
            else
            {
                response.Result.CommandMessage = "This invitation is invalid";
            }
            return response;
        }
    }
}
