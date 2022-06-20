using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.ProjectActivity.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.ListProjectActivity
{
    public class ListProjectActivityQuery : IRequest<ValidateableResponse<ApiResponse<List<ProjectActivityDto>>>>
    {
        #region Props
        [DataMember]
        public int ProjectId { get; set; }
        #endregion

        #region CTRS
        public ListProjectActivityQuery(int id)
        {
            ProjectId = id;
        }
        #endregion
    }
}
