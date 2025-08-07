using System;
using Shouldly;

namespace Alphicsh.Applikite.Files.Tests.FilePaths;

public class FilePath_AppendTests
{
    // ------------------
    // Append with rooted
    // ------------------

    [Fact]
    public void Append_ShouldReplaceRootedWithRooted()
    {
        var basePath = new FilePath(FileRoot.Location + "Lorem/Ipsum");
        var appendedPath = basePath.Append(FileRoot.Location + "Dolor/sit.txt");
        appendedPath.Value.ShouldBe(FileRoot.Location + "Dolor/sit.txt");
    }

    [Fact]
    public void Append_ShouldReplaceRelativeWithRooted()
    {
        var basePath = new FilePath("Lorem/Ipsum");
        var appendedPath = basePath.Append(FileRoot.Location + "Dolor/sit.txt");
        appendedPath.Value.ShouldBe(FileRoot.Location + "Dolor/sit.txt");
    }

    // ---------------------------
    // Append rooted with relative
    // ---------------------------

    [Fact]
    public void Append_ShouldAppendRootedWithRelative()
    {
        var basePath = new FilePath(FileRoot.Location + "Lorem/Ipsum");
        var appendedPath = basePath.Append("Dolor/sit.txt");
        appendedPath.Value.ShouldBe(FileRoot.Location + "Lorem/Ipsum/Dolor/sit.txt");
    }

    [Fact]
    public void Append_ShouldAppendRootedWithBacktracking()
    {
        var basePath = new FilePath(FileRoot.Location + "Lorem/Ipsum");
        var appendedPath = basePath.Append("..");
        appendedPath.Value.ShouldBe(FileRoot.Location + "Lorem");
    }

    [Fact]
    public void Append_ShouldAppendRootedWithBackAndForth()
    {
        var basePath = new FilePath(FileRoot.Location + "Lorem/Ipsum");
        var appendedPath = basePath.Append("../Dolor");
        appendedPath.Value.ShouldBe(FileRoot.Location + "Lorem/Dolor");
    }

    // -----------------------------
    // Append relative with relative
    // -----------------------------

    [Fact]
    public void Append_ShouldAppendRelativeWithRelative()
    {
        var basePath = new FilePath("Lorem/Ipsum");
        var appendedPath = basePath.Append("Dolor/sit.txt");
        appendedPath.Value.ShouldBe("Lorem/Ipsum/Dolor/sit.txt");
    }

    [Fact]
    public void Append_ShouldAppendRelativeWithBacktracking()
    {
        var basePath = new FilePath("Lorem/Ipsum");
        var appendedPath = basePath.Append("..");
        appendedPath.Value.ShouldBe("Lorem");
    }

    [Fact]
    public void Append_ShouldBacktrackRelativeToBaseDirectory()
    {
        var basePath = new FilePath("Lorem/Ipsum");
        var appendedPath = basePath.Append("../..");
        appendedPath.Value.ShouldBe("");
    }

    [Fact]
    public void Append_ShouldAppendRelativeWithBackAndForth()
    {
        var basePath = new FilePath("Lorem/Ipsum");
        var appendedPath = basePath.Append("../Dolor");
        appendedPath.Value.ShouldBe("Lorem/Dolor");
    }

    [Fact]
    public void Append_ShouldAppendRelativeToBaseDirectory()
    {
        var basePath = new FilePath("Lorem/Ipsum");
        var appendedPath = basePath.Append("../../Dolor");
        appendedPath.Value.ShouldBe("Dolor");
    }

    [Fact]
    public void Append_ShouldThrowWhenLeavingBaseDirectory()
    {
        var basePath = new FilePath("Lorem/Ipsum");
        Action appendAction = () => basePath.Append("../../..");
        appendAction.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void Append_ShouldThrowWhenLeavingBaseDirectoryWithBaseGuard()
    {
        // __BASE is a special directory used for checking
        // whether the base directory was left or not
        // if appended path contains __BASE, underscores are added
        // until special name doesn't appear in subpath to be appended

        // this test ensures that __BASE check can't be circumvented too easily
        var basePath = new FilePath("Lorem/Ipsum");
        Action appendAction = () => basePath.Append("../../../__BASE");
        appendAction.ShouldThrow<ArgumentException>();
    }

    // ---------
    // Overloads
    // ---------

    [Fact]
    public void Append_ShouldAcceptFilePath()
    {
        var basePath = new FilePath("Lorem/Ipsum");
        var relativePath = new FilePath("Dolor/sit.txt");
        var appendedPath = basePath.Append(relativePath);
        appendedPath.Value.ShouldBe("Lorem/Ipsum/Dolor/sit.txt");
    }

    [Fact]
    public void TryAppend_ShouldAppendString()
    {
        var basePath = new FilePath("Lorem/Ipsum");
        var appendedPath = basePath.TryAppend("Dolor/sit.txt")!.Value;
        appendedPath.Value.ShouldBe("Lorem/Ipsum/Dolor/sit.txt");
    }

    [Fact]
    public void TryAppend_ShouldHandleNullString()
    {
        var basePath = new FilePath("Lorem/Ipsum");
        var appendedPath = basePath.TryAppend((string?)null);
        appendedPath.ShouldBeNull();
    }

    [Fact]
    public void TryAppend_ShouldAppendFilePath()
    {
        var basePath = new FilePath("Lorem/Ipsum");
        var relativePath = new FilePath("Dolor/sit.txt");
        var appendedPath = basePath.TryAppend(relativePath)!.Value;
        appendedPath.Value.ShouldBe("Lorem/Ipsum/Dolor/sit.txt");
    }

    [Fact]
    public void TryAppend_ShouldHandleNullPath()
    {
        var basePath = new FilePath("Lorem/Ipsum");
        var appendedPath = basePath.TryAppend((FilePath?)null);
        appendedPath.ShouldBeNull();
    }
}

