using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.DeleteProject
{
    public class DeleteProjectCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        [DataMember]
        public int Id { get; set; }
        #endregion

        #region Method
        public DeleteProjectCommand(int id)
        {
            Id = id;
        }
        #endregion

    }
}
