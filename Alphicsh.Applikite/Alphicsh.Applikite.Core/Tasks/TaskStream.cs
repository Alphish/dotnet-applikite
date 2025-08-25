using System.Threading.Tasks;
using Alphicsh.Applikite.Models;
using Alphicsh.Applikite.Tasks.Channels;
using Alphicsh.Applikite.Tasks.Progress;
using Alphicsh.Applikite.Tasks.Sources;

namespace Alphicsh.Applikite.Tasks;

public class TaskStream<TResult, TProgress> : ITaskStream<TResult, TProgress>
    where TProgress : IProgressData<TProgress>
{
    private ITaskSource<TResult> TaskSource { get; }
    private ITaskChannel<TResult> TaskChannel { get; }
    private IProgressHandler<TProgress> ProgressHandler { get; }

    public IValueSource<TResult> ResultSource { get; }
    public IValueSource<TProgress> ProgressSource { get; }

    public TaskStream(ITaskSource<TResult> source, ITaskChannel<TResult> channel)
    {
        TaskSource = source;
        TaskChannel = channel;

        ResultSource = ValueSource.Create(channel.Result);
        TaskChannel.TaskCompleted += (sender, e) => ResultSource.Value = e;

        ProgressSource = ValueSource.Create(TProgress.CreateDefault());
        ProgressHandler = channel.GetProgressHandler<TProgress>();
        ProgressHandler.ProgressChanged += (sender, e) => ProgressSource.Value = e;
    }

    public void SendTask()
    {
        var task = TaskSource.Create();
        TaskChannel.AcceptTask(task);
    }

    public void Cancel()
    {
        TaskChannel.Cancel();
        ProgressSource.Value = TProgress.CreateDefault();
    }

    public async Task<TResult> GetResultAsync()
    {
        return await TaskChannel.GetResultAsync();
    }
}
