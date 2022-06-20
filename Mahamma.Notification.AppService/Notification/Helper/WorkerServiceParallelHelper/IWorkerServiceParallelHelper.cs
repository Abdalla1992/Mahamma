using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.Helper.WorkerServiceParallelHelper
{
    public interface IWorkerServiceParallelHelper
    {
        Task HandleProcess<T>(List<T> listToProcess, Func<T, int, CancellationTokenSource, Task> funcToExecute,
            CancellationToken workerServiceCancellationToken, bool useParallel, int threadCount);
    }
}
