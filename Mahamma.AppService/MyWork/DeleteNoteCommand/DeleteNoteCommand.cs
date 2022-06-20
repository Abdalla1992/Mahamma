using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;

namespace Mahamma.AppService.MyWork.DeleteNoteCommand
{
    public class DeleteNoteCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        public int Id { get; set; }
    }
}
