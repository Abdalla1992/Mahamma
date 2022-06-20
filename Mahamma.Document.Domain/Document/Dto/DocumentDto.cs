using Mahamma.Base.Dto.Dto;
using System;
using System.IO;

namespace Mahamma.Document.Domain.Document.Dto
{
    public class DocumentDto
    {
        #region Props
        public MemoryStream MemoryStream { get; set; }
        public string ContentType { get; set; }
        public string FileDownloadName { get; set; }
        #endregion
        #region Nav Prop
        #endregion
    }
}
