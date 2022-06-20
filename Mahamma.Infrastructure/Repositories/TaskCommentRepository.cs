using AutoMapper;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.Enum;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Task.Dto;
using Mahamma.Domain.Task.Entity;
using Mahamma.Domain.Task.Repository;
using Mahamma.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Repositories
{
    public class TaskCommentRepository : Base.EntityRepository<TaskComment>, ITaskCommentRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public TaskCommentRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }
        public void AddComment(TaskComment comment)
        {
            CreateAsyn(comment);
        }
        public async Task<bool> ValidToLikeComment(int taskId, int commentId)
        {
            return await GetAnyAsync(t => t.TaskId == taskId && t.Id == commentId);
        }

        public async Task<TaskComment> GetEntityById(int commentId)
        {
            return await FirstOrDefaultAsync(t => t.Id == commentId);
        }
        public async Task<List<TaskCommentDto>> GetTaskComment(int taskId)
        {
            List<TaskComment> taskComments = await GetWhereAsync(c => c.TaskId == taskId && !c.ParentCommentId.HasValue,
                "Replies,Likes,Replies.Likes");
            return Mapper.Map<List<TaskCommentDto>>(taskComments);
        }
    }
}
