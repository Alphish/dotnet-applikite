using System;
using System.Threading.Tasks;
using Alphicsh.Applikite.Tasks.Progress;

namespace Alphicsh.Applikite.Tasks.Channels;

public class LastTaskChannel<TResult> : ITaskChannel<TResult>
{
    private ProgressManager Progress { get; }
    private IManagedTask<TResult> LastTask { get; set; }
    public TResult Result { get; private set; }
    public event EventHandler<TResult>? TaskCompleted;

    public LastTaskChannel(TResult startingValue)
    {
        Progress = new ProgressManager();
        Progress.SetSender(this);

        LastTask = ManagedTask.FromResult(startingValue);
        Result = startingValue;
    }

    public LastTaskChannel() : this(default!) { }

    public void AcceptTask(IManagedTask<TResult> task)
    {
        LastTask.Cancel();

        LastTask = task;
        LastTask.LinkProgress(Progress);
        LastTask.TaskCompleted += (sender, result) =>
        {
            if (sender == LastTask)
                TaskCompleted?.Invoke(this, result);
        };
    }

    public void Cancel()
    {
        LastTask.Cancel();
    }

    public IProgressHandler<TProgress> GetProgressHandler<TProgress>()
        => new ProgressHandler<TProgress>(Progress);

    public async Task<TResult> GetResultAsync()
    {
        bool isResolved;
        do
        {
            isResolved = await TryResolveLastTask();
        } while (!isResolved);

        return Result;
    }

    private async Task<bool> TryResolveLastTask()
    {
        var nextTask = LastTask;
        await nextTask;

        if (nextTask != LastTask)
            return false;

        if (LastTask.RanToCompletion)
        {
            Result = nextTask.Result!;
        }

        return true;
    }
}

