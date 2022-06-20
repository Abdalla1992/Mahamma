using AutoMapper;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Project.Entity;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Repositories
{
    public class ProjectLikeCommentRepository : Base.EntityRepository<Domain.Project.Entity.ProjectLikeComment>, IProjectLikeCommentRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public ProjectLikeCommentRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }

        public async Task LikeComment(int memberId, int commentId)
        {
            ProjectLikeComment like = await FirstOrDefaultAsync(c => c.ProjectCommentId == commentId && c.ProjectMemberId == memberId);
            if (like == null)
            {
                like = new();
                like.LikeAComment(commentId, memberId);
                CreateAsyn(like);
            }
            else
            {
                Delete(like);
            }
        }
    }
}
