using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Document.Domain.Document.Dto
{
    public class UploadResponse
    {
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public string ActualFileName { get; set; }
    }
}
