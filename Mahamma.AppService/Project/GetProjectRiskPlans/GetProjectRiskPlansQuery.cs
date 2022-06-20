using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.Project.Dto;
using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Project.GetProjectRiskPlans
{
    public class GetProjectRiskPlansQuery : IRequest<ValidateableResponse<ApiResponse<List<ProjectRiskPlanDto>>>>
    {
        [DataMember]
        public int ProjectId { get; set; }

        public GetProjectRiskPlansQuery(int projectId)
        {
            ProjectId = projectId;
        }
    }
}
