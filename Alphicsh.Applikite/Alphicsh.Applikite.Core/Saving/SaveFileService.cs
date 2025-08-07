using System.Threading;
using System.Threading.Tasks;
using Alphicsh.Applikite.Files;

namespace Alphicsh.Applikite.Saving;

public class SaveFileService : ISaveFileService
{
    private IFilesystem Filesystem { get; }

    public string PendingSuffix { get; init; } = ".new";

    public SaveFileService(IFilesystem filesystem)
    {
        Filesystem = filesystem;
    }

    public async Task Save(FilePath path, string content, CancellationToken cancellationToken = default)
    {
        // making sure there's no leftover ongoing save state
        Clean(path);

        // create a preliminary version
        // making a separate file to avoid corrupting the original file
        // if the operations stops partway through
        var pendingPath = new FilePath(path.Value + PendingSuffix);
        await Filesystem.WriteFile(pendingPath, content, cancellationToken);

        // after the entire file has been successfully written
        // move it to the target position
        Filesystem.MoveFile(pendingPath, path);
    }

    public void Clean(FilePath path)
    {
        var pendingPath = new FilePath(path.Value + PendingSuffix);
        if (Filesystem.FileExists(pendingPath))
        {
            Filesystem.DeleteFile(pendingPath);
        }
    }

    public async Task<string?> Load(FilePath path, CancellationToken cancellationToken = default)
    {
        Clean(path);
        if (!Filesystem.FileExists(path))
            return null;

        return await Filesystem.ReadFile(path, cancellationToken);
    }
}
