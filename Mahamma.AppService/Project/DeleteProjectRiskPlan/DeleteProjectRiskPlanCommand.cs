using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Project.DeleteProjectRiskPlan
{
    public class DeleteProjectRiskPlanCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        [DataMember]
        public int Id { get; set; }
        #endregion

        public DeleteProjectRiskPlanCommand(int id)
        {
            Id = id;
        }
    }
}
