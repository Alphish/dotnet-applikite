using System;
using System.Threading.Tasks;
using Alphicsh.Applikite.Tasks.Progress;

namespace Alphicsh.Applikite.Tasks.Channels;

public interface ITaskChannel<TResult>
{
    void AcceptTask(IManagedTask<TResult> task);

    void Cancel();
    IProgressHandler<TProgress> GetProgressHandler<TProgress>();

    Task<TResult> GetResultAsync();
    TResult Result { get; }
    event EventHandler<TResult>? TaskCompleted;
}
