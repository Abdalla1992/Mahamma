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
    public class TaskMemberRepository : Base.EntityRepository<TaskMember>, ITaskMemberRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public TaskMemberRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }
        public async Task<TaskMemberDto> GetMember(int taskId, long userId)
        {
            var member = await FirstOrDefaultAsync(m => m.UserId == userId && m.TaskId == taskId);
            return Mapper.Map<TaskMemberDto>(member);
        }
        public async Task<List<long>> GetMembersUserIdList(int taskId)
        {
            return (await GetWhereAsync(t => t.TaskId == taskId)).Select(m => m.UserId).ToList();
        }
        public async Task<List<TaskMember>> GetMembersList(int taskId, List<long> userIdList)
        {
            return (await GetWhereAsync(t => t.TaskId == taskId && userIdList.Contains(t.UserId))).ToList();
        }
        public void AssingMemberListToTask(List<TaskMember> newMembers)
        {
            CreateListAsyn(newMembers);
        }

        public async Task<IQueryable<TaskDto>> GetHistoryTasks(long userId)
        {
            IEnumerable<Domain.Task.Entity.Task> memberTasks = (await GetWhereAsync(m => m.UserId == userId, "Task")).Select(t => t.Task);
            return Mapper.Map<IQueryable<TaskDto>>(memberTasks);
        }

        public async Task<List<TaskMember>> GetTaskMemberByTaskId(int taskId)
        {
            return await GetWhereAsync(m => m.TaskId == taskId);
        }

        public async Task<TaskMember> GetById(int taskMemberId)
        {
            return await FirstOrDefaultAsync(t => t.Id == taskMemberId);
        }

        public async Task<List<TaskMember>> GetTaskMemberByTaskIdExceptCurrentUser(int taskId ,long currentUser)
        {
            return await GetWhereAsync(m => m.TaskId == taskId && m.UserId !=currentUser);
        }

    }
}
