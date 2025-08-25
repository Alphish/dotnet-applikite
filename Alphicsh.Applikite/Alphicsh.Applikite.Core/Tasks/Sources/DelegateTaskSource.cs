using System;
using System.Threading;
using System.Threading.Tasks;

namespace Alphicsh.Applikite.Tasks.Sources;

public class DelegateTaskSource<TResult> : BaseTaskSource<TResult>
{
    private Func<CancellationToken, IProgress<object>, Task<TResult>> RunDelegate { get; }

    public DelegateTaskSource(Func<CancellationToken, IProgress<object>, Task<TResult>> runDelegate)
    {
        RunDelegate = runDelegate;
    }

    public override Task<TResult> Run(CancellationToken cancellationToken, IProgress<object> progress)
        => RunDelegate(cancellationToken, progress);
}

public static class DelegateTaskSource
{
    public static DelegateTaskSource<TResult> Of<TResult>(Func<CancellationToken, IProgress<object>, Task<TResult>> runDelegate)
        => new DelegateTaskSource<TResult>(runDelegate);
}
