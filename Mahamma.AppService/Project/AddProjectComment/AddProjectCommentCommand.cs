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

namespace Mahamma.AppService.Project.AddProjectComment
{
    public class AddProjectCommentCommand : IRequest<ValidateableResponse<ApiResponse<List<ProjectCommentDto>>>>
    {
        #region Props
        [DataMember]
        public int ProjectId { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public List<long> MentionedUserList { get; set; }
        [DataMember]
        public int? ParentCommentId { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
        #endregion
    }
}
