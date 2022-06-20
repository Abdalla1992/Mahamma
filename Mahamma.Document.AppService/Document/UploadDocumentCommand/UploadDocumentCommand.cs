using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Document.Domain.Document.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Runtime.Serialization;

namespace Mahamma.Document.AppService.Document.UploadDocumentCommand
{
    public class UploadDocumentCommand : IRequest<UploadResponse>
    {
        #region Props
        [DataMember]
        public IFormFile file { get; set; }
        #endregion
    }
}
