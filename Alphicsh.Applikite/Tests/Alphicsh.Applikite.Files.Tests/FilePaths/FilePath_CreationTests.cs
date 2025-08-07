using System;
using Shouldly;

namespace Alphicsh.Applikite.Files.Tests.FilePaths;

public class FilePath_CreationTests
{
    [Fact]
    public void FilePath_ShouldStoreGivenPath()
    {
        var givenValue = "C:/Lorem/Ipsum";
        var filePath = new FilePath(givenValue);

        var expectedValue = "C:/Lorem/Ipsum";
        filePath.Value.ShouldBe(expectedValue);
    }

    [Fact]
    public void FilePath_ShouldEnforceForeslashSeparators()
    {
        var givenValue = @"C:\Lorem\Ipsum";
        var filePath = new FilePath(givenValue);

        var expectedValue = "C:/Lorem/Ipsum";
        filePath.Value.ShouldBe(expectedValue);
    }

    [Fact]
    public void FilePath_ShouldRejectNullString()
    {
        string givenValue = null!;
        Action constructorAction = () => new FilePath(givenValue);
        constructorAction.ShouldThrow<ArgumentNullException>();
    }

    [Fact]
    public void FilePath_ShouldCreateFromString()
    {
        var givenValue = @"C:\Lorem\Ipsum";
        var filePath = FilePath.From(givenValue);

        var expectedValue = "C:/Lorem/Ipsum";
        filePath.Value.ShouldBe(expectedValue);
    }

    [Fact]
    public void FilePath_ShouldCreateFromNullableString()
    {
        var givenValue = @"C:\Lorem\Ipsum";
        var filePath = FilePath.TryFrom(givenValue)!.Value;

        var expectedValue = "C:/Lorem/Ipsum";
        filePath.Value.ShouldBe(expectedValue);
    }

    [Fact]
    public void FilePath_ShouldTryCreateFromNull()
    {
        string? givenValue = null;
        var filePath = FilePath.TryFrom(givenValue);
        filePath.ShouldBeNull();
    }
}

