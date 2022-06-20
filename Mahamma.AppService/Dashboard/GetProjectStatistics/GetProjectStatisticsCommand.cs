using Mahamma.AppService.Dashboard.Dto;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.MemberSearch.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.MemberSearch.GetProjectStatistics
{
    public class GetProjectStatisticsCommand : IRequest<ValidateableResponse<ApiResponse<DashboardDto>>>
    {
        #region Prop
        [DataMember]
        public List<int> ProjectIdList { get; set; }
        #endregion
    }
}
