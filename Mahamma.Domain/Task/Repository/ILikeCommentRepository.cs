using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Task.Dto;
using Mahamma.Domain.Task.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mahamma.Domain.Task.Repository
{
    public interface ILikeCommentRepository : IRepository<Entity.LikeComment>
    {
        System.Threading.Tasks.Task LikeComment(int memberId, int commentId);
    }
}
