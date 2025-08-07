using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Alphicsh.Applikite.Files.Tests;

public class FakeFilesystem : IFilesystem
{
    private Dictionary<string, string> Files { get; } = new Dictionary<string, string>();

    public bool FileExists(FilePath path)
    {
        return Files.ContainsKey(path.ToString().ToLowerInvariant());
    }

    public Task<string> ReadFile(FilePath path, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Files[path.ToString().ToLowerInvariant()]);
    }

    public Task WriteFile(FilePath path, string content, CancellationToken cancellationToken = default)
    {
        Files[path.ToString().ToLowerInvariant()] = content;
        return Task.CompletedTask;
    }

    public void MoveFile(FilePath pathFrom, FilePath pathTo)
    {
        Files[pathTo.ToString().ToLowerInvariant()] = Files[pathFrom.ToString().ToLowerInvariant()];
        Files.Remove(pathFrom.ToString().ToLowerInvariant());
    }

    public void DeleteFile(FilePath path)
    {
        Files.Remove(path.ToString().ToLowerInvariant());
    }
}
