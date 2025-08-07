using System.Threading;
using System.Threading.Tasks;

namespace Alphicsh.Applikite.Files;

public interface IFilesystem
{
    bool FileExists(FilePath path);
    Task<string> ReadFile(FilePath path, CancellationToken cancellationToken = default);
    Task WriteFile(FilePath path, string content, CancellationToken cancellationToken = default);
    void MoveFile(FilePath pathFrom, FilePath pathTo);
    void DeleteFile(FilePath path);
}
