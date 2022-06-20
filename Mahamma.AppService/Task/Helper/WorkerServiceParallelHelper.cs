using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Mahamma.AppService.Task.Helper
{
    public class WorkerServiceParallelHelper : IWorkerServiceParallelHelper
    {
        public ILogger<IWorkerServiceParallelHelper> Logger { get; }

        public WorkerServiceParallelHelper(ILogger<IWorkerServiceParallelHelper> logger)
        {
            Logger = logger;
        }
        #region Public Methods
        public async System.Threading.Tasks.Task HandleProcess<T>(List<T> listToProcess, Func<T, CancellationTokenSource, System.Threading.Tasks.Task> funcToExecute, CancellationToken workerServiceCancellationToken, bool useParallel, int threadCount)
        {
            if (useParallel && threadCount > default(int))
                await RunProcessInParallelWithTasks(listToProcess, funcToExecute, threadCount, workerServiceCancellationToken);
            else
                await RunProcessInSequence(listToProcess, funcToExecute, workerServiceCancellationToken);
        }
        #endregion

        #region Private Methods
        private async System.Threading.Tasks.Task RunProcessInParallelWithTasks<T>(List<T> listToProcess, Func<T, CancellationTokenSource, System.Threading.Tasks.Task> funcToExecute, int threadCount, CancellationToken workerServiceCancellationToken)
        {
            try
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                await System.Threading.Tasks.Task.WhenAll(SplitList(listToProcess, threadCount)
                    .Select(list => System.Threading.Tasks.Task.Run(() =>
                    {
                        foreach (var item in list.TakeWhile(item => !cancellationTokenSource.IsCancellationRequested && !workerServiceCancellationToken.IsCancellationRequested))
                            funcToExecute(item, cancellationTokenSource);
                    }, cancellationTokenSource.Token)));
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "An Error Occured While executing RunProcessInParallelWithSystem.Threading.Tasks.Tasks");
            }
        }
        private async System.Threading.Tasks.Task RunProcessInSequence<T>(List<T> listToProcess, Func<T, CancellationTokenSource, System.Threading.Tasks.Task> funcToExecute, CancellationToken workerServiceCancellationToken)
        {
            try
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                foreach (var item in listToProcess.TakeWhile(item => !cancellationTokenSource.IsCancellationRequested && !workerServiceCancellationToken.IsCancellationRequested))
                    await funcToExecute(item, cancellationTokenSource);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "An Error Occured While executing RunProcessInSequence");
            }
        }
        private IEnumerable<List<T>> SplitList<T>(List<T> listToSplit, int splitCount = 5)
        {
            for (int i = 0; i < splitCount; i++)
            {
                int takeCount = (int)Math.Ceiling((double)listToSplit.Count / splitCount);
                yield return listToSplit.Skip(i * takeCount).Take(takeCount).ToList();
            }
        }
        #endregion
    }
}
