using System;
using System.IO;
using Shouldly;

namespace Alphicsh.Applikite.Files.Tests.FilePaths;

public class FilePath_InfoTests
{
    [Fact]
    public void IsRelative_ShouldBeTrueForRelativePath()
    {
        var path = new FilePath("Lorem/Ipsum");
        path.IsRelative.ShouldBeTrue();
    }

    [Fact]
    public void IsRelative_ShouldBeFalseForRootedPath()
    {
        var path = new FilePath(Path.Combine(FileRoot.Location, "Test"));
        path.IsRelative.ShouldBeFalse();
    }

    [Fact]
    public void IsAbsolute_ShouldBeFalseForRelativePath()
    {
        var path = new FilePath("Lorem/Ipsum");
        path.IsAbsolute.ShouldBeFalse();
    }

    [Fact]
    public void IsAbsolute_ShouldBeTrueForRootedPath()
    {
        var path = new FilePath(Path.Combine(FileRoot.Location, "Test"));
        path.IsAbsolute.ShouldBeTrue();
    }

    [Fact]
    public void Filename_ShouldReturnFilenameWithExtension()
    {
        var path = new FilePath("Lorem/Ipsum/dolor.txt");
        path.Filename.ShouldBe("dolor.txt");
    }

    [Fact]
    public void Filename_ShouldReturnFilenameWithoutExtension()
    {
        var path = new FilePath("Lorem/Ipsum");
        path.Filename.ShouldBe("Ipsum");
    }

    [Fact]
    public void FilenameWithoutExtension_ShouldReturnPartForFilenameWithExtension()
    {
        var path = new FilePath("Lorem/Ipsum/dolor.txt");
        path.FilenameWithoutExtension.ShouldBe("dolor");
    }

    [Fact]
    public void FilenameWithoutExtension_ShouldReturnFilenameGivenNoExtension()
    {
        var path = new FilePath("Lorem/Ipsum");
        path.FilenameWithoutExtension.ShouldBe("Ipsum");
    }

    [Fact]
    public void Extension_ShouldReturnForFilenameWithExtension()
    {
        var path = new FilePath("Lorem/Ipsum/dolor.txt");
        path.Extension.ShouldBe(".txt");
    }

    [Fact]
    public void Extension_ShouldBeEmptyGivenNoExtension()
    {
        var path = new FilePath("Lorem/Ipsum");
        path.Extension.ShouldBeEmpty();
    }

    [Fact]
    public void GetDirectory_ShouldReturnParentPath()
    {
        var path = new FilePath("Lorem/Ipsum/dolor.txt");
        path.GetDirectory().Value.ShouldBe("Lorem/Ipsum");
    }

    [Fact]
    public void GetDirectory_ShouldBeEmptyForSingleSegmentPath()
    {
        var path = new FilePath("Lorem");
        path.GetDirectory().Value.ShouldBe("");
    }

    [Fact]
    public void GetDirectory_ShouldThrowForEmptyPath()
    {
        var path = new FilePath("");
        Action directoryRetrieval = () => path.GetDirectory();
        directoryRetrieval.ShouldThrow<InvalidOperationException>();
    }

    [Fact]
    public void GetDirectory_ShouldThrowForRoot()
    {
        var path = new FilePath(FileRoot.Location);
        Action directoryRetrieval = () => path.GetDirectory();
        directoryRetrieval.ShouldThrow<InvalidOperationException>();
    }

    [Fact]
    public void TryGetDirectory_ShouldReturnParentPath()
    {
        var path = new FilePath("Lorem/Ipsum/dolor.txt");
        var directoryPath = path.TryGetDirectory()!.Value;
        directoryPath.Value.ShouldBe("Lorem/Ipsum");
    }

    [Fact]
    public void TryGetDirectory_ShouldBeEmptyForSingleSegmentPath()
    {
        var path = new FilePath("Lorem");
        var directoryPath = path.TryGetDirectory()!.Value;
        directoryPath.Value.ShouldBe("");
    }

    [Fact]
    public void TryGetDirectory_ShouldBeNullForEmptyPath()
    {
        var path = new FilePath("");
        path.TryGetDirectory().ShouldBeNull();
    }

    [Fact]
    public void TryGetDirectory_ShouldBeNullForRoot()
    {
        var path = new FilePath(FileRoot.Location);
        path.TryGetDirectory().ShouldBeNull();
    }
}
