using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.Language.Dto;
using Mahamma.Identity.Domain.Language.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Language.GetLanguage
{
    public class GetLanguageQueryHandler : IRequestHandler<GetLanguageQuery, ValidateableResponse<ApiResponse<LanguageDto>>>
    {
        private readonly ILanguageRepository _languageRepository;
        public GetLanguageQueryHandler(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public async Task<ValidateableResponse<ApiResponse<LanguageDto>>> Handle(GetLanguageQuery request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<LanguageDto>> response = new(new ApiResponse<LanguageDto>());
            response.Result.ResponseData = await _languageRepository.GetLanguageById(request.Id);
            return response;
        }
    }
}
