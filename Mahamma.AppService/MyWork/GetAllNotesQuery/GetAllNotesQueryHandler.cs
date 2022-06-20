using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.MyWork.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mahamma.Domain.MyWork.Repository;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Identity.ApiClient.Dto.User;

namespace Mahamma.AppService.MyWork.GetAllNotesQuery
{
    public class GetAllNotesQueryHandler : IRequestHandler<GetAllNotesQuery, ValidateableResponse<ApiResponse<List<NoteDto>>>>
    {
        #region Props
        private readonly IHttpContextAccessor _httpContext;
        private readonly INoteRepository _noteRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        #endregion

        #region CTRS
        public GetAllNotesQueryHandler(IHttpContextAccessor httpContext, INoteRepository noteRepository,
                                        IMessageResourceReader messageResourceReader)
        {
            _httpContext = httpContext;
            _noteRepository = noteRepository;
            _messageResourceReader = messageResourceReader;
        }

        public async Task<ValidateableResponse<ApiResponse<List<NoteDto>>>> Handle(GetAllNotesQuery request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<List<NoteDto>>> response = new(new ApiResponse<List<NoteDto>>());
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];

            List<NoteDto> notes = await _noteRepository.GetAllNotes(currentUser.Id);

            if (notes != null && notes.Count > 0)
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("ProcessCompletedSuccessfully", currentUser.LanguageId);
                response.Result.ResponseData = notes;
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("NoDataFound", currentUser.LanguageId);
            }

            return response;
        }
        #endregion

    }
}
