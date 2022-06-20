using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.Language.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Language.GetLanguage
{
    public class GetLanguageQuery : IRequest<ValidateableResponse<ApiResponse<LanguageDto>>>
    {
        [DataMember]
        public int Id { get; set; }
        public GetLanguageQuery(int id)
        {
            Id = id;
        }
    }
}
