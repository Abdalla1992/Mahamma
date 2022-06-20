using Mahamma.Base.Dto.Dto;

namespace Mahamma.Domain.ProjectAttachment.Dto
{
    public class FolderDto : BaseDto<int>
    {
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public int TaskId { get; set; }

        #region Methods
        public string CleanName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Name))
                    return string.Empty;
                else
                    return RemoveWhiteSpace(Name.ToLower().Trim());
            }
        }
        #endregion
    }
}
