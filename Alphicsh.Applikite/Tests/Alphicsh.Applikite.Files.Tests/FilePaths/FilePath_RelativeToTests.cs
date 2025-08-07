using System;
using Shouldly;

namespace Alphicsh.Applikite.Files.Tests.FilePaths;

public class FilePath_RelativeToTests
{
    [Fact]
    public void RelativeTo_ShouldReturnRelativeToItself()
    {
        var originPath = FileRoot.Path.Append("Lorem/Ipsum/dolor.txt");
        var targetPath = FileRoot.Path.Append("Lorem/Ipsum/dolor.txt");
        var relativePath = targetPath.RelativeTo(originPath);
        var expectedPath = new FilePath("");
        relativePath.ShouldBe(expectedPath);
    }

    [Fact]
    public void RelativeTo_ShouldHandleSubpath()
    {
        var originPath = FileRoot.Path.Append("Lorem");
        var targetPath = FileRoot.Path.Append("Lorem/Ipsum/dolor.txt");
        var relativePath = targetPath.RelativeTo(originPath);
        var expectedPath = new FilePath("Ipsum/dolor.txt");
        relativePath.ShouldBe(expectedPath);
    }

    [Fact]
    public void RelativeTo_ShouldHandleAncestor()
    {
        var originPath = FileRoot.Path.Append("Lorem/Ipsum/dolor.txt");
        var targetPath = FileRoot.Path.Append("Lorem");
        var relativePath = targetPath.RelativeTo(originPath);
        var expectedPath = new FilePath("../..");
        relativePath.ShouldBe(expectedPath);
    }

    [Fact]
    public void RelativeTo_ShouldHandleCousin()
    {
        var originPath = FileRoot.Path.Append("Lorem/Ipsum/dolor.txt");
        var targetPath = FileRoot.Path.Append("Lorem/sit.txt");
        var relativePath = targetPath.RelativeTo(originPath);
        var expectedPath = new FilePath("../../sit.txt");
        relativePath.ShouldBe(expectedPath);
    }

    [Fact]
    public void RelativeTo_ShouldThrowInvalidOperationExceptionOnRelativeTarget()
    {
        var originPath = FileRoot.Path.Append("Lorem");
        var targetPath = new FilePath("Lorem/Ipsum/dolor.txt");
        Action relativePathAction = () => targetPath.RelativeTo(originPath);
        relativePathAction.ShouldThrow<InvalidOperationException>();
    }

    [Fact]
    public void RelativeTo_ShouldThrowArgumentExceptionOnRelativeOrigin()
    {
        var originPath = new FilePath("Lorem");
        var targetPath = FileRoot.Path.Append("Lorem/Ipsum/dolor.txt");
        Action relativePathAction = () => targetPath.RelativeTo(originPath);
        relativePathAction.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void RelativeTo_ShouldAcceptString()
    {
        var originPath = FileRoot.Path.Append("Lorem").Value;
        var targetPath = FileRoot.Path.Append("Lorem/Ipsum/dolor.txt");
        var relativePath = targetPath.RelativeTo(originPath);
        var expectedPath = new FilePath("Ipsum/dolor.txt");
        relativePath.ShouldBe(expectedPath);
    }
}
