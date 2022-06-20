using AutoMapper;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.TaskActivity.Dto;
using Mahamma.Domain.TaskActivity.Entity;
using Mahamma.Domain.TaskActivity.Repository;
using Mahamma.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Repositories
{
    public class TaskActivityRepository : Base.EntityRepository<TaskActivity>, ITaskActivityRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public TaskActivityRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }

        public void LogActivity(TaskActivity activity)
        {
            CreateAsyn(activity);
        }

        public async Task<List<TaskActivityDto>> GetLogs(int taskId)
        {
            var result = await GetWhereNoTrackingAsync(g => g.TaskId == taskId);
            return Mapper.Map<List<TaskActivityDto>>(result);
        }
    }
}
