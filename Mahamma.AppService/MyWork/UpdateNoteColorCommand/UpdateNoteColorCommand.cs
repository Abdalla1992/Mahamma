using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;

namespace Mahamma.AppService.MyWork.UpdateColorCommand
{
    public class UpdateNoteColorCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        public int Id { get; set; }
        public string ColorCode { get; set; }
    }
}
