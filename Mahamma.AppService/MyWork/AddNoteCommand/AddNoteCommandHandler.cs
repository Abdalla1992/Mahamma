using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.MyWork.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.MyWork.AddNoteCommand
{
    public class AddNoteCommandHandler : IRequestHandler<AddNoteCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly IHttpContextAccessor _httpContext;
        private readonly INoteRepository _noteRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        #endregion

        public AddNoteCommandHandler(IHttpContextAccessor httpContext, INoteRepository noteRepository,
                                        IMessageResourceReader messageResourceReader)
        {
            _httpContext = httpContext;
            _noteRepository = noteRepository;
            _messageResourceReader = messageResourceReader;
        }
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(AddNoteCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];

            request.OwnerId = currentUser.Id;
            _noteRepository.AddNote(request);

            if (await _noteRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                response.Result.ResponseData = true;
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("DataAddededSuccessfully", currentUser.LanguageId);
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToAddTheNewData", currentUser.LanguageId);
            }

            return response;

        }
    }
}
