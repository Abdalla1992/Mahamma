using Mahamma.Domain._SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Project.Repositroy
{
    public interface IProjectLikeCommentRepository : IRepository<Entity.ProjectLikeComment>
    {
        System.Threading.Tasks.Task LikeComment(int memberId, int commentId);

    }
}
