using System.Threading;
using System.Threading.Tasks;
using Alphicsh.Applikite.Files;

namespace Alphicsh.Applikite.Saving;

public interface ISaveFileService
{
    Task Save(FilePath path, string content, CancellationToken cancellationToken = default);
    void Clean(FilePath path);
    Task<string?> Load(FilePath path, CancellationToken cancellationToken = default);
}
