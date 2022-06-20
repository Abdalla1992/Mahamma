using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Company.Entity;
using Mahamma.Domain.Company.Repositroy;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Company.UpdateCompanyInvitationStatus
{
    public class UpdateCompanyInvitationStatusCommandHandler : IRequestHandler<UpdateCompanyInvitationStatusCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly ICompanyInvitationRepository _companyInvitationRepository;

        #endregion

        #region Ctor
        public UpdateCompanyInvitationStatusCommandHandler(ICompanyInvitationRepository companyInvitationRepository)
        {
            _companyInvitationRepository = companyInvitationRepository;

        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(UpdateCompanyInvitationStatusCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            CompanyInvitation companyInvitation = await _companyInvitationRepository.GetCompanyInvitationEntityByInvitationId(request.InvitationId);
            if (companyInvitation != null)
            {
                companyInvitation.UpdateInvitationStatus(request.InvitationStatusId);
                _companyInvitationRepository.UpdateCompanyInvitation(companyInvitation);
                if (await _companyInvitationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = "Data updated successfully";
                }
                else
                {
                    response.Result.CommandMessage = "Failed to modify Language. Try again shortly.";
                }
            }
            else
            {
                response.Result.CommandMessage = "This invitation is invalid";
            }
            return response;
        }
    }
}
