using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Folder.DeleteFolder
{
    public class DeleteFolderCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        [DataMember]
        public int FolderId { get; set; }
        #endregion

        #region Ctor
        public DeleteFolderCommand(int id)
        {
            FolderId = id;
        }
        #endregion
    }
}
