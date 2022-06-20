using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.Project.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.GetProjectTaskSubtaskNames
{
    public class GetProjectTaskSubtaskNamesQuery : IRequest<ValidateableResponse<ApiResponse<ProjectTaskSubtaskNamesDto>>>
    {
        [DataMember]
        public int ProjectId { get; set; }
        [DataMember]
        public int? TaskId { get; set; }
    }
}
