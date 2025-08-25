namespace Alphicsh.Applikite.Tasks.Sources;

public interface ITaskSource<TResult>
{
    IManagedTask<TResult> Create();
}
