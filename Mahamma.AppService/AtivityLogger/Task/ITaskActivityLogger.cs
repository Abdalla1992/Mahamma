using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.AtivityLogger.Task
{
    public interface ITaskActivityLogger
    {
        void LogTaskActivity(string action, int taskId, int memberId, CancellationToken cancellationToken);
        System.Threading.Tasks.Task LogTaskActivity(string action, int taskId, long userId, CancellationToken cancellationToken);
    }
}
