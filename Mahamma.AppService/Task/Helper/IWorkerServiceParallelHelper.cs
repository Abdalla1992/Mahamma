using System;
using System.Collections.Generic;
using System.Threading;

namespace Mahamma.AppService.Task.Helper
{
    public interface IWorkerServiceParallelHelper
    {
        System.Threading.Tasks.Task HandleProcess<T>(List<T> listToProcess, Func<T, CancellationTokenSource, System.Threading.Tasks.Task> funcToExecute,
            CancellationToken workerServiceCancellationToken, bool useParallel, int threadCount);
    }
}
