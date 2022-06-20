using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.ProjectAttachment.Entity
{
   public class Folder : Entity<int> , IAggregateRoot
    {
        public string Name { get; private set; }
        public int  ProjectId { get; private set; }
        public int?  TaskId { get; private set; }

        #region Navigation Prop
        public Domain.Project.Entity.Project Project { get; private set; }
        public Domain.Task.Entity.Task Task { get; private set; }
        public virtual List<FolderFile> FolderFiles { get; private set; }
        #endregion

        public void CreateFolder(string name , int projectId , int? taskId)
        {
            Name = name;
            ProjectId = projectId;
            TaskId = taskId;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }
        public void RenameFolder(string name)
        {
            Name = name;
        }
        public void DeleteFolder()
        {
            foreach (var folderFile in FolderFiles)
            {
                folderFile.DeleteFile(deleteFile : FolderFiles.Where(f => f.DeletedStatus == Base.Dto.Enum.DeletedStatus.NotDeleted.Id).Count() == 1);
            }
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }

    }
}
