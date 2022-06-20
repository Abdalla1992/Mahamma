using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Meeting.Repositroy;
using Mahamma.Domain.Project.Dto;
using Mahamma.Domain.Project.Repositroy;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.GetProjectData
{
   public class GetProjectDataHandler : IRequestHandler<GetProjectDataQuery, ValidateableResponse<ApiResponse<ProjectDto>>>
    {
        #region Prop
        private readonly IProjectRepository _projectRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IMeetingRepository _meetingRepository;


        #endregion

        #region Ctor
        public GetProjectDataHandler(IProjectRepository projectRepository, IMessageResourceReader messageResourceReader,
            IMeetingRepository meetingRepository)
        {
            _projectRepository = projectRepository;
            _messageResourceReader = messageResourceReader;
            _meetingRepository = meetingRepository;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<ProjectDto>>> Handle(GetProjectDataQuery request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<ProjectDto>> response = new(new ApiResponse<ProjectDto>());

            ProjectDto projectDto = await _projectRepository.GetProjectDataById(request.Id);
            if (projectDto !=null)
            {
                response.Result.ResponseData = projectDto;
                response.Result.CommandMessage = "Process completed successfully.";
            }
            else
            {
                response.Result.CommandMessage = "No data found.";
            }
            return response;
        }
    }
}
