using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.ProjectAttachment.Dto;
using Mahamma.Domain.ProjectAttachment.Repository;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.ListTaskFiles
{
    public class ListTaskFilesQueryHandler : IRequestHandler<ListTaskFilesQuery, ValidateableResponse<ApiResponse<List<ProjectAttachmentDto>>>>
    {
        #region Props
        private readonly IProjectAttachmentRepository _projectAttachmentRepository;
        #endregion

        #region CTRS
        public ListTaskFilesQueryHandler(IProjectAttachmentRepository projectAttachmentRepository)
        {
            _projectAttachmentRepository = projectAttachmentRepository;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<List<ProjectAttachmentDto>>>> Handle(ListTaskFilesQuery request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<List<ProjectAttachmentDto>>> response = new(new ApiResponse<List<ProjectAttachmentDto>>());
            var taskFilesList = await _projectAttachmentRepository.GetTaskFilesList(request.TaskId);

            if (taskFilesList != null && taskFilesList.Count > 0)
            {
                response.Result.CommandMessage = $"{taskFilesList.Count} file found";
                response.Result.ResponseData = taskFilesList;
            }
            else
            {
                response.Result.CommandMessage = $"No date found.";
            }
            return response;
        }
    }
}
