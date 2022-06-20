using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Document.Domain.Document.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Runtime.Serialization;

namespace Mahamma.Document.AppService.Document.DownloadDocumentCommand
{
    public class DownloadDocumentCommand : IRequest<ValidateableResponse<ApiResponse<DocumentDto>>>
    {
        #region Props
        [DataMember]
        public string FileName { get; set; }
        #endregion
    }
}
