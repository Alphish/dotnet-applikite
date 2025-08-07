using System.Threading;
using System.Threading.Tasks;
using Shouldly;

namespace Alphicsh.Applikite.Files.Tests;

public class FakeFilesystem_Tests
{
    private CancellationToken CancellationToken => TestContext.Current.CancellationToken;

    [Fact]
    public async Task FakeFilesystem_ShouldManageFiles()
    {
        var filesystem = new FakeFilesystem();
        var fooPath = new FilePath("/foo.txt");
        var barPath = new FilePath("/bar.txt");

        filesystem.FileExists(fooPath).ShouldBeFalse();
        filesystem.FileExists(barPath).ShouldBeFalse();

        await filesystem.WriteFile(fooPath, "Hello!", CancellationToken);

        filesystem.FileExists(fooPath).ShouldBeTrue();
        (await filesystem.ReadFile(fooPath, CancellationToken)).ShouldBe("Hello!");
        filesystem.FileExists(barPath).ShouldBeFalse();

        filesystem.MoveFile(fooPath, barPath);

        filesystem.FileExists(fooPath).ShouldBeFalse();
        filesystem.FileExists(barPath).ShouldBeTrue();
        (await filesystem.ReadFile(barPath, CancellationToken)).ShouldBe("Hello!");

        filesystem.DeleteFile(barPath);

        filesystem.FileExists(fooPath).ShouldBeFalse();
        filesystem.FileExists(barPath).ShouldBeFalse();
    }
}

