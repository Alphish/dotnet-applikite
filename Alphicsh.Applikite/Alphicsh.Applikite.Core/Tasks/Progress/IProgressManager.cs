using System;

namespace Alphicsh.Applikite.Tasks.Progress;

public interface IProgressManager : IProgress<object>
{
    event EventHandler<object>? ProgressChanged;
    void SetSender(object sender);
    void Cancel();
    void Link(IProgress<object> manager);
}
