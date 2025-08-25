using System;

namespace Alphicsh.Applikite.Tasks.Progress;

public interface IProgressHandler<TProgress>
{
    event EventHandler<TProgress>? ProgressChanged;
}
