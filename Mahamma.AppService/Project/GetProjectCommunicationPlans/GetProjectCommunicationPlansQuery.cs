using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.Project.Dto;
using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Project.GetProjectCommunicationPlans
{
    public class GetProjectCommunicationPlansQuery : IRequest<ValidateableResponse<ApiResponse<List<ProjectCommunicationPlanDto>>>>
    {
        [DataMember]
        public int ProjectId { get; set; }

        public GetProjectCommunicationPlansQuery(int projectId)
        {
            ProjectId = projectId;
        }
    }
}
