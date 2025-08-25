namespace Alphicsh.Applikite.Tasks.Progress;

public interface IProgressData<TProgress> where TProgress : IProgressData<TProgress>
{
    static abstract TProgress CreateDefault();
}
