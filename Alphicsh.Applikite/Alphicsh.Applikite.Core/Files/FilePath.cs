using System;
using System.IO;
using System.Text.Json.Serialization;

namespace Alphicsh.Applikite.Files;

[JsonConverter(typeof(FilePathJsonConverter))]
public record struct FilePath
{
    public string Value { get; }

    public FilePath(string path)
    {
        ArgumentNullException.ThrowIfNull(path, nameof(path));

        // store path normalised to the same directory separator char
        Value = path.NormalizePath();
    }

    public static FilePath From(string path)
        => new FilePath(path);

    public static FilePath? TryFrom(string? path)
        => path != null ? new FilePath(path) : null;

    public override string ToString()
        => Value;

    public static implicit operator string(FilePath path) => path.Value;

    public Uri ToUri()
        => new Uri(Value);

    public static implicit operator Uri(FilePath path) => new Uri(path.Value);

    // ----------------
    // Path information
    // ----------------

    public bool IsAbsolute
        => Path.IsPathRooted(Value);

    public bool IsRelative
        => !Path.IsPathRooted(Value);

    public string Filename
        => Path.GetFileName(Value);

    public string FilenameWithoutExtension
        => Path.GetFileNameWithoutExtension(Value);

    public string Extension
        => Path.GetExtension(Value);

    public FilePath? TryGetDirectory()
        => FilePath.TryFrom(Path.GetDirectoryName(Value));

    public FilePath GetDirectory()
        => TryGetDirectory() ?? throw new InvalidOperationException($"The path '{Value}' doesn't have a parent directory.");

    // --------
    // Relative
    // --------

    public FilePath RelativeTo(FilePath originPath)
    {
        if (IsRelative)
            throw new InvalidOperationException($"Path '{Value}' cannot be used as a relative path target, because it's relative.");

        if (originPath.IsRelative)
            throw new ArgumentException($"Path '{originPath.Value}' cannot be used as a relative path origin, because it's relative.", nameof(originPath));

        var result = Path.GetRelativePath(originPath, Value);

        // if the origin and target are the same, replace the dot with an empty string
        if (result == ".")
            return new FilePath("");

        return new FilePath(result);
    }

    public FilePath RelativeTo(string originPath)
        => RelativeTo(FilePath.From(originPath));

    // ---------
    // Appending
    // ---------

    public FilePath Append(string relativePath)
    {
        // an absolute path overrides the current path
        if (Path.IsPathRooted(relativePath))
            return new FilePath(relativePath);

        // the relative path is simple appended to an absolute path
        if (IsAbsolute)
            return new FilePath(Path.GetFullPath(relativePath, this.Value));

        // when relative path is appended to a relative path
        // a rooted path is artificially created to perform path operations on
        // also, the appended path must not leave the scope of the current path's directory

        var baseDirectory = "__BASE";
        while (relativePath.Contains(baseDirectory, StringComparison.OrdinalIgnoreCase))
        {
            baseDirectory += "_";
        }
        var baseRoot = Path.Combine(FileRoot.Location, baseDirectory).NormalizePath();
        var rootedPath = Path.Combine(baseRoot, Value).NormalizePath();
        var combinedPath = Path.GetFullPath(relativePath, rootedPath).NormalizePath();

        if (!combinedPath.StartsWith(baseRoot))
            throw new ArgumentException($"The path '{relativePath}' goes outside the directory of '{Value}' and thus cannot be appended.", nameof(relativePath));

        var appendedPath = combinedPath.Substring(baseRoot.Length).TrimStart('/');
        return new FilePath(appendedPath);
    }

    public FilePath Append(FilePath relativePath)
        => Append(relativePath.Value);

    public FilePath? TryAppend(string? relativePath)
        => relativePath != null ? Append(relativePath) : null;

    public FilePath? TryAppend(FilePath? relativePath)
        => relativePath != null ? Append(relativePath.Value) : null;
}
