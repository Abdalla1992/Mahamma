using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.MyWork.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.MyWork.DeleteNoteCommand
{
    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly IHttpContextAccessor _httpContext;
        private readonly INoteRepository _noteRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        #endregion

        public DeleteNoteCommandHandler(IHttpContextAccessor httpContext, INoteRepository noteRepository,
                                        IMessageResourceReader messageResourceReader)
        {
            _httpContext = httpContext;
            _noteRepository = noteRepository;
            _messageResourceReader = messageResourceReader;
        }

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];

            _noteRepository.DeleteNote(request.Id);

            if (await _noteRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                response.Result.ResponseData = true;
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("DataDeletedSuccessfully", currentUser.LanguageId);
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToDelete", currentUser.LanguageId);
            }
            return response;
        }
    }
}
