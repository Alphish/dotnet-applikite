using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Alphicsh.Applikite.Files;

// cannot be easily tested, as it handles actual files
[ExcludeFromCodeCoverage]
public class Filesystem : IFilesystem
{
    public bool FileExists(FilePath path)
        => File.Exists(path);

    public Task<string> ReadFile(FilePath path, CancellationToken cancellationToken = default)
        => File.ReadAllTextAsync(path, cancellationToken);

    public Task WriteFile(FilePath path, string content, CancellationToken cancellationToken = default)
        => File.WriteAllTextAsync(path, content, cancellationToken);

    public void MoveFile(FilePath pathFrom, FilePath pathTo)
        => File.Move(pathFrom, pathTo, overwrite: true);

    public void DeleteFile(FilePath path)
        => File.Delete(path);
}
