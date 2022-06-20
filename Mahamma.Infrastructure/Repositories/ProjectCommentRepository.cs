using AutoMapper;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Project.Dto;
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
    public class ProjectCommentRepository : Base.EntityRepository<ProjectComment>, IProjectCommentRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public ProjectCommentRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }

        public async Task<ProjectComment> GetEntityById(int commentId)
        {
            return await FirstOrDefaultAsync(t => t.Id == commentId);
        }

        public void AddComment(ProjectComment comment)
        {
            CreateAsyn(comment);
        }

        public async Task<bool> ValidToLikeComment(int projectId, int commentId)
        {
            return await GetAnyAsync(t => t.ProjectId == projectId && t.Id == commentId);
        }

        public async Task<List<ProjectCommentDto>> GetProjectComment(int projectId)
        {
            List<ProjectComment> projectComments = await GetWhereAsync(c => c.ProjectId == projectId && !c.ParentCommentId.HasValue,
                "Replies,Likes,Replies.Likes");
            return Mapper.Map<List<ProjectCommentDto>>(projectComments);
        }
    }
}
