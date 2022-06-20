using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Runtime.Serialization;

namespace Mahamma.Document.AppService.Document.DeleteDocumentCommand
{
    public class DeleteDocumentCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        [DataMember]
        public string fileName { get; set; }
        #endregion
    }
}
