using System.Threading.Tasks;
using Alphicsh.Applikite.Models;

namespace Alphicsh.Applikite.Tasks;
public interface ITaskStream<TResult, TProgress>
{
    IValueSource<TResult> ResultSource { get; }
    IValueSource<TProgress> ProgressSource { get; }

    void SendTask();
    void Cancel();
    Task<TResult> GetResultAsync();
}