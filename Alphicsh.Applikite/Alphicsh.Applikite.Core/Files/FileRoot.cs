using System;

using IOPath = System.IO.Path;

namespace Alphicsh.Applikite.Files;

public static class FileRoot
{
    public static string Location { get; } =
        IOPath.GetPathRoot(typeof(FileRoot).Assembly.Location)?.NormalizePath()
        ?? throw new NotSupportedException("Could not determine a reference file root.");

    public static FilePath Path { get; } = new FilePath(Location);
}

