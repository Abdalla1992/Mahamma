using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.AtivityLogger.Project
{
      public interface IProjectActivityLogger
    {
        void LogProjectActivity(string action, int ProjectId, int projectMemberId, CancellationToken cancellationToken);
        System.Threading.Tasks.Task LogProjectActivity(string action, int projectId, long userId , CancellationToken cancellationToken);
    }
}
