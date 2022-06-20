using Mahamma.Base.Domain;
using System;

namespace Mahamma.Domain.MyWork.Entity
{
    public class Note : Entity<int>, IAggregateRoot
    {
        #region Props
        public string Body { get; set; }
        public string Title { get; set; }
        public string ColorCode { get; set; }
        public bool IsTask { get; set; }
        public long OwnerId { get; set; }
        #endregion

        #region Methods
        public void CreateNote()
        {
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }
        public void DeleteNote()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }
        #endregion
    }
}
