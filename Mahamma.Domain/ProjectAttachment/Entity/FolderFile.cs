using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.ProjectAttachment.Entity
{
    public class FolderFile : Entity<int>, IAggregateRoot
    {
        public int FolderId { get; private set; }
        public int ProjectAttachmentId { get; private set; }

        #region Navigation Prop
        public virtual ProjectAttachment ProjectAttachment { get; private set; }
        public virtual Folder Folder { get; private set; }
        #endregion


        public void CreateFolderFile(int folderId, int projectAttachmentId)
        {
            FolderId = folderId;
            ProjectAttachmentId = projectAttachmentId;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }

        public void MoveFile(int folderId)
        {
            FolderId = folderId;
        }

        public void DeleteFile(bool deleteFile)
        {
            if (deleteFile)
            {
                ProjectAttachment.DeleteAttachment();
            }
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }
    }
}
