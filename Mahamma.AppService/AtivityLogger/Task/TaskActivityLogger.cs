using Mahamma.Domain.Task.Repository;
using Mahamma.Domain.TaskActivity.Entity;
using Mahamma.Domain.TaskActivity.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.AtivityLogger.Task
{
    public class TaskActivityLogger : ITaskActivityLogger
    {
        ITaskActivityRepository _taskActivityRepository { get; set; }
        ITaskMemberRepository _taskMemberRepository { get; set; }
        public TaskActivityLogger(ITaskActivityRepository taskActivityRepository, ITaskMemberRepository taskMemberRepository)
        {
            _taskActivityRepository = taskActivityRepository;
            _taskMemberRepository = taskMemberRepository;
        }
        public void LogTaskActivity(string action, int taskId, int memberId, CancellationToken cancellationToken)
        {
            TaskActivity activity = new();
            activity.LogActivity(action, taskId, memberId);

            _taskActivityRepository.LogActivity(activity);
        }
        public async System.Threading.Tasks.Task LogTaskActivity(string action, int taskId, long userId, CancellationToken cancellationToken)
        {
            var member = await _taskMemberRepository.GetMember(taskId, userId);
            if (member != null)
            {
                TaskActivity activity = new();
                activity.LogActivity(action, taskId, member.Id);

                _taskActivityRepository.LogActivity(activity);
            }
        }
    }
}
