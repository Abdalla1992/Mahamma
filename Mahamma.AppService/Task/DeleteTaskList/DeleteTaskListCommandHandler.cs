using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Task.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.DeleteTaskList
{
    public class DeleteTaskListCommandHandler : IRequestHandler<DeleteTaskListCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly ITaskRepository _taskRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;


        #endregion

        #region CTRS
        public DeleteTaskListCommandHandler(ITaskRepository taskRepository, IMessageResourceReader messageResourceReader,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext)
        {
            _taskRepository = taskRepository;
            _messageResourceReader = messageResourceReader;
            _httpContext = httpContext;

        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(DeleteTaskListCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];

            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());

            await _taskRepository.DeleteTaskList(request.TaskIdList);

            if (await _taskRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                response.Result.ResponseData = true;
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("DataDeletedSuccessfully",currentUser.LanguageId );
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToDeleteTheTaskList", currentUser.LanguageId);
            }
            return response;
        }
    }
}
