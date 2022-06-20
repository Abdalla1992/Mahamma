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

namespace Mahamma.AppService.Project.GetProjectData
{
   public class GetProjectDataQuery : IRequest<ValidateableResponse<ApiResponse<ProjectDto>>>
    {
        #region Props
        [DataMember]
        public int Id { get; set; }
        #endregion

        #region CTRS
        public GetProjectDataQuery(int id)
        {
            Id = id;
        }
        #endregion
    }
}
