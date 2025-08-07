using System;
using Shouldly;

namespace Alphicsh.Applikite.Files.Tests.FilePaths;

public class FilePath_ConversionTests
{
    // --------------
    // Path to string
    // --------------

    [Fact]
    public void ToString_ShouldReturnPathValue()
    {
        var path = new FilePath("Lorem/Ipsum");
        path.Value.ShouldBe("Lorem/Ipsum");

        var str = path.ToString();
        str.ShouldBe("Lorem/Ipsum");
    }

    [Fact]
    public void CastString_ShouldReturnPathValue()
    {
        var path = new FilePath("Lorem/Ipsum");
        CheckPathString(path, "Lorem/Ipsum");
    }

    private void CheckPathString(string pathString, string expectedString)
    {
        pathString.ShouldBe(expectedString);
    }

    // -----------
    // Path to URI
    // -----------

    [Fact]
    public void ToUri_ShouldUsePathValue()
    {
        var path = new FilePath(FileRoot.Location + "Lorem/Ipsum");
        var uri = path.ToUri();
        var expectedUri = new Uri(FileRoot.Location + "Lorem/Ipsum");
        uri.ShouldBe(expectedUri);
    }

    [Fact]
    public void CastUri_ShouldUsePathValue()
    {
        var path = new FilePath(FileRoot.Location + "Lorem/Ipsum");
        var expectedUri = new Uri(FileRoot.Location + "Lorem/Ipsum");
        CheckPathUri(path, expectedUri);
    }

    private void CheckPathUri(Uri pathUri, Uri expectedUri)
    {
        pathUri.ShouldBe(expectedUri);
    }
}

