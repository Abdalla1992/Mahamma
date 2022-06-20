using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mahamma.Domain.ProjectAttachment.Entity
{
    public class ProjectAttachment : Entity<int>, IAggregateRoot
    {
        #region Props
        public string FileName { get; private set; }
        public string FileUrl { get; private set; }
        public int ProjectId { get; private set; }
        public int? TaskId { get; private set; }
        #endregion

        public virtual List<FolderFile> FolderFiles { get; private set; }


        #region Methods
        public void CreateAttachment(string url, int projectId, int? taskId, string fileName)
        {
            FileUrl = url;
            ProjectId = projectId;
            TaskId = taskId;
            FileName = fileName;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
          
        }
        public void DeleteAttachment()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }
        public void MoveFile(int oldFolderId, int newFolderId)
        {
            FolderFile oldFolderFile = FolderFiles.FirstOrDefault(f => f.FolderId == oldFolderId);
            oldFolderFile.MoveFile(newFolderId);
        }
        #endregion
    }
}
