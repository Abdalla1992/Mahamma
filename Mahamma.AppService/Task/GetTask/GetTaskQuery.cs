using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.Task.Dto;
using MediatR;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Task.GetTask
{
    public class GetTaskQuery : IRequest<ValidateableResponse<ApiResponse<TaskDto>>>
    {
        #region Props
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public bool RequestedFromMeeting { get; set; }
        #endregion

        #region CTRS
        public GetTaskQuery(int id, bool requestedFromMeeting)
        {
            Id = id;
            RequestedFromMeeting = requestedFromMeeting;
        }
        #endregion
    }
}
