using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.MyWork.Dto;
using MediatR;
using System.Collections.Generic;

namespace Mahamma.AppService.MyWork.GetAllNotesQuery
{
    public class GetAllNotesQuery : IRequest<ValidateableResponse<ApiResponse<List<NoteDto>>>>
    {
        public int Id { get; set; }
    }
}
