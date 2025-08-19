using System;

namespace Alphicsh.Applikite.Tasks.Progress;

public class ProgressHandler<TProgress> : IProgressHandler<TProgress>
{
    private IProgressManager SourceProgress { get; }
    public event EventHandler<TProgress>? ProgressChanged;

    public ProgressHandler(IProgressManager sourceProgress)
    {
        SourceProgress = sourceProgress;
        SourceProgress.ProgressChanged += (sender, e) =>
        {
            if (e is not TProgress typedProgress)
                return;

            ProgressChanged?.Invoke(sender, typedProgress);
        };
    }
}
