using System;
using Alphicsh.Applikite.Tasks.Progress;

namespace Alphicsh.Applikite.Tasks;

public interface IManagedTask<TResult> : IAwaitable<TResult>
{
    void Cancel();
    IProgressHandler<TProgress> GetProgressHandler<TProgress>();
    void LinkProgress(IProgress<object> progress);
}
