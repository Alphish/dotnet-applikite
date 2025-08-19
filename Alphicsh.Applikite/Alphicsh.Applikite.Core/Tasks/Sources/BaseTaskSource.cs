using System;
using System.Threading;
using System.Threading.Tasks;
using Alphicsh.Applikite.Tasks.Progress;

namespace Alphicsh.Applikite.Tasks.Sources;

public abstract class BaseTaskSource<TResult> : ITaskSource<TResult>
{
    public IManagedTask<TResult> Create()
    {
        var cancellationSource = new CancellationTokenSource();
        var progress = new ProgressManager();
        var task = Run(cancellationSource.Token, progress);
        return new ManagedTask<TResult>(cancellationSource, progress, task);
    }

    public abstract Task<TResult> Run(CancellationToken cancellationToken, IProgress<object> progress);
}
