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

namespace Mahamma.AppService.Project.GetProject
{
    public class GetProjectQuery : IRequest<ValidateableResponse<ApiResponse<ProjectUserDto>>>
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public bool RequestedFromMeeting { get; set; }

        public GetProjectQuery(int id, bool requestedFromMeeting)
        {
            Id = id;
            RequestedFromMeeting = requestedFromMeeting;
        }

    }
}
