using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.Project.Dto;
using MediatR;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Project.GetProjectCharter
{
    public class GetProjectCharterQuery : IRequest<ValidateableResponse<ApiResponse<ProjectCharterDto>>>
    {
        [DataMember]
        public int ProjectId { get; set; }

        public GetProjectCharterQuery(int projectId)
        {
            ProjectId = projectId;
        }
    }
}
