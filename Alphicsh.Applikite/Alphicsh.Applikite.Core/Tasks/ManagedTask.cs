using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Alphicsh.Applikite.Tasks.Progress;

namespace Alphicsh.Applikite.Tasks;

public class ManagedTask<TResult> : IManagedTask<TResult>
{
    protected Task<TResult> InnerTask { get; set; }
    private CancellationTokenSource CancellationSource { get; }
    private IProgressManager Progress { get; }
    public TResult? Result { get; private set; }
    public bool RanToCompletion { get; private set; }
    public event EventHandler<TResult>? TaskCompleted;

    public ManagedTask(CancellationTokenSource cancellationSource, IProgressManager progress, Task<TResult> task)
    {
        CancellationSource = cancellationSource;
        Progress = progress;
        Progress.SetSender(this);
        InnerTask = task.ContinueWith(ReportCompletion, TaskContinuationOptions.OnlyOnRanToCompletion);
    }

    public TaskAwaiter<TResult> GetAwaiter()
        => InnerTask.GetAwaiter();

    public IProgressHandler<TProgress> GetProgressHandler<TProgress>()
        => new ProgressHandler<TProgress>(Progress);

    public void LinkProgress(IProgress<object> progress)
        => Progress.Link(progress);

    public void Cancel()
    {
        Progress.Cancel();
        CancellationSource.Cancel();
    }

    private TResult ReportCompletion(Task<TResult> task)
    {
        RanToCompletion = true;
        TaskCompleted?.Invoke(this, task.Result);
        return task.Result;
    }
}

public static class ManagedTask
{
    public static ManagedTask<TResult> FromResult<TResult>(TResult result)
    {
        var cancellationSource = new CancellationTokenSource();
        var progress = new ProgressManager();
        var task = Task.FromResult(result);
        return new ManagedTask<TResult>(cancellationSource, progress, task);
    }
}
