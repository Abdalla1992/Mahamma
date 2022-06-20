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

namespace Mahamma.AppService.Project.LikeProjectComment
{
    public class LikeProjectCommentCommand : IRequest<ValidateableResponse<ApiResponse<List<ProjectCommentDto>>>>
    {
        #region Props
        [DataMember]
        public int CommentId { get; set; }
        [DataMember]
        public int ProjectId { get; set; }
        #endregion
    }
}
