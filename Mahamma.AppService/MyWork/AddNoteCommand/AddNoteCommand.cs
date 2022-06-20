using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.MyWork.Dto;
using MediatR;

namespace Mahamma.AppService.MyWork.AddNoteCommand
{
    public class AddNoteCommand : NoteDto, IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
    }
}
