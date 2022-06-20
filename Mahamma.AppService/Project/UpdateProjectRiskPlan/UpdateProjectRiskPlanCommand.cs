using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Project.UpdateProjectRiskPlan
{
    public class UpdateProjectRiskPlanCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int ProjectId { get; set; }
        [DataMember]
        public string Issue { get; set; }
        [DataMember]
        public string Impact { get; set; }
        [DataMember]
        public string Action { get; set; }
        [DataMember]
        public string Owner { get; set; }
        #endregion
    }
}
