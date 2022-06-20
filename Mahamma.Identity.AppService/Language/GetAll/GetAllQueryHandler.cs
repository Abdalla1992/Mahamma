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

namespace Mahamma.Identity.AppService.Language.GetAll
{
    public class GetAllQueryHandler : IRequestHandler<GetAllQuery, ValidateableResponse<ApiResponse<List<LanguageDto>>>>
    {
        private readonly ILanguageRepository _languageRepository;
        public GetAllQueryHandler(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public async Task<ValidateableResponse<ApiResponse<List<LanguageDto>>>> Handle(GetAllQuery request,CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<List<LanguageDto>>> response = new(new ApiResponse<List<LanguageDto>>());
            response.Result.ResponseData = await _languageRepository.GetAllLanguages();
            return response;
        }
    }
}
