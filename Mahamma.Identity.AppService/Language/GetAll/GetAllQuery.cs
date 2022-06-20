using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.Language.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Language.GetAll
{
    public class GetAllQuery : IRequest<ValidateableResponse<ApiResponse<List<LanguageDto>>>>
    {
        public GetAllQuery() { }
    }
}
