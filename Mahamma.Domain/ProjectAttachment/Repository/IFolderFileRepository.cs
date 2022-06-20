using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.ProjectAttachment.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.ProjectAttachment.Repository
{
    public interface IFolderFileRepository : IRepository<Entity.FolderFile>
    {
        void AddFolderFiles(List<FolderFile> folderFiles);
        void AddFolderFile(FolderFile folderFile);
    }
}
