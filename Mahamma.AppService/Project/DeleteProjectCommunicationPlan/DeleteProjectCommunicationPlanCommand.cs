using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Project.DeleteProjectCommunicationPlan
{
    public class DeleteProjectCommunicationPlanCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        [DataMember]
        public int Id { get; set; }
        #endregion

        public DeleteProjectCommunicationPlanCommand(int id)
        {
            Id = id;
        }
    }
}
