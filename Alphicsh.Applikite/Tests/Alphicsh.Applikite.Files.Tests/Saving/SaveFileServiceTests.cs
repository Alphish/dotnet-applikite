using System.Threading.Tasks;
using Alphicsh.Applikite.Saving;
using Shouldly;

namespace Alphicsh.Applikite.Files.Tests.Saving;

public class SaveFileServiceTests
{
    private IFilesystem Filesystem { get; }
    private SaveFileService SaveFileService { get; }

    public SaveFileServiceTests()
    {
        Filesystem = new FakeFilesystem();
        SaveFileService = new SaveFileService(Filesystem);
    }

    [Fact]
    public async Task Load_ShouldBeEmptyForNonexistent()
    {
        var path = new FilePath(@"C:\Lorem\thisfiledoesnotexist.txt");
        var result = await SaveFileService.Load(path, TestContext.Current.CancellationToken);
        result.ShouldBeNull();
    }

    [Fact]
    public async Task Load_ShouldHaveContentForExisting()
    {
        var path = new FilePath(@"C:\Lorem\test.txt");
        await Filesystem.WriteFile(path, "THIS IS TEST", TestContext.Current.CancellationToken);

        var result = await SaveFileService.Load(path, TestContext.Current.CancellationToken);
        result.ShouldBe("THIS IS TEST");
    }

    [Fact]
    public async Task Load_ShouldHaveContentForSavedFile()
    {
        var path = new FilePath(@"C:\Lorem\test.txt");
        await SaveFileService.Save(path, "THIS IS TEST", TestContext.Current.CancellationToken);

        var result = await SaveFileService.Load(path, TestContext.Current.CancellationToken);
        result.ShouldBe("THIS IS TEST");
    }

    [Fact]
    public async Task Load_ShouldCleanupForUnconfirmedPending()
    {
        var loadPath = new FilePath(@"C:\Lorem\test.txt");
        var pendingPath = new FilePath(@"C:\Lorem\test.txt.new");
        await Filesystem.WriteFile(pendingPath, "THIS IS TEST", TestContext.Current.CancellationToken);

        Filesystem.FileExists(pendingPath).ShouldBeTrue();
        var result = await SaveFileService.Load(loadPath, TestContext.Current.CancellationToken);
        result.ShouldBeNull();
        Filesystem.FileExists(pendingPath).ShouldBeFalse();
    }

    [Fact]
    public async Task Load_ShouldCleanupForAlternativePending()
    {
        var altPendingService = new SaveFileService(Filesystem) { PendingSuffix = ".pending" };

        var loadPath = new FilePath(@"C:\Lorem\test.txt");
        var newPath = new FilePath(@"C:\Lorem\test.txt.new");
        var pendingPath = new FilePath(@"C:\Lorem\test.txt.pending");
        await Filesystem.WriteFile(newPath, "THIS IS TEST", TestContext.Current.CancellationToken);
        await Filesystem.WriteFile(pendingPath, "THIS IS TEST", TestContext.Current.CancellationToken);

        Filesystem.FileExists(newPath).ShouldBeTrue();
        Filesystem.FileExists(pendingPath).ShouldBeTrue();

        var result = await altPendingService.Load(loadPath, TestContext.Current.CancellationToken);
        result.ShouldBeNull();

        Filesystem.FileExists(newPath).ShouldBeTrue();
        Filesystem.FileExists(pendingPath).ShouldBeFalse();
    }
}

