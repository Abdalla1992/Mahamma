using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Document.AppService.Document.Helper
{
    public class FileHelper : IFileHelper
    {
        #region CTRS
        public FileHelper()
        {
        }
        #endregion

        public FileStream GetFileStream(string filePath, string directory)
        {
            FileStream fileStream = null;
            if (File.Exists(string.Concat(directory, $"/{filePath}")))
                fileStream = File.OpenRead(Path.Combine(directory, filePath));

            return fileStream;
        }

        public void SaveFile(string base64FileContent, string directory, string fileName)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            byte[] buffer = Encoding.ASCII.GetBytes(base64FileContent);
            MemoryStream ms = new MemoryStream(buffer);
            //write to file
            FileStream file = new FileStream(directory + "/" + fileName, FileMode.Create, FileAccess.Write);
            ms.WriteTo(file);
            file.Close();
            ms.Close();
        }

        public async Task<string> SaveFileAsync(IFormFile formFile, string directory, string subDirectory = null)
        {
            string filePath = string.Empty;

            string fullDirectory = directory;
            if (!string.IsNullOrWhiteSpace(subDirectory))
                fullDirectory = string.Concat(directory, $"/{subDirectory}");

            if (!Directory.Exists(fullDirectory))
                Directory.CreateDirectory(fullDirectory);

            DirectoryInfo myDir = new DirectoryInfo(fullDirectory);
            int fileCount = myDir.GetFiles().Length + 1;

            using (var file = File.Create(string.Concat(fullDirectory, $"/{fileCount.ToString()}") + $"{Path.GetExtension(formFile.FileName)}"))
            {
                await formFile.CopyToAsync(file);

                if (!string.IsNullOrEmpty(subDirectory))
                    filePath = string.Concat(subDirectory, $"/{Path.GetFileName(file.Name)}");
                else
                    filePath = Path.GetFileName(file.Name);
            }

            return filePath;
        }

        public async Task<MemoryStream> GetFileStream(string filePath)
        {
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return memory;
        }

        public string GetContentType(string fileName)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return types[ext];
        }

        public bool DeleteFile(string path)
        {
            bool result = default;
            if (File.Exists(path))
                File.Delete(path);
                result = !File.Exists(path);
            return result;
        }

        #region Private Methods
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats.officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".svg", "image/svg+xml"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".mp3", "audio/mpeg"}
            };
        }

        #endregion
    }
}
