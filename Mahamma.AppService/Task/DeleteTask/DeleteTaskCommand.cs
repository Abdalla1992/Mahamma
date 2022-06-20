using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using System.Runtime.Serialization;
using MediatR;

namespace Mahamma.AppService.Task.DeleteTask
{
    public class DeleteTaskCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        [DataMember]
        public int Id { get; set; }
        #endregion

        #region CTRS
        public DeleteTaskCommand(int id)
        {
            Id = id;
        }
        #endregion
    }
}
