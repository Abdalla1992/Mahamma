using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Project.Dto;
using Mahamma.Domain.Project.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Project.Repositroy
{
    public interface IProjectCommentRepository : IRepository<Entity.ProjectComment>
    {
        Task<ProjectComment> GetEntityById(int commentId);
        void AddComment(ProjectComment comment);
        Task<bool> ValidToLikeComment(int taskId, int commentId);
        Task<List<ProjectCommentDto>> GetProjectComment(int projectId);
    }
}
