using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.Task.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.LikeComment
{
    public class LikeCommentCommand : IRequest<ValidateableResponse<ApiResponse<List<TaskCommentDto>>>>
    {
        #region Props
        [DataMember]
        public int CommentId { get; set; }
        [DataMember]
        public int TaskId { get; set; }
        #endregion
    }
}
