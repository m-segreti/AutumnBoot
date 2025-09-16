namespace Server.Services;

/// <summary>
/// Basic, minimal, file operations for access to a local filesystem.
/// </summary>
public interface ISimpleFileService
{
    /// <summary>
    /// Save a string of content to the path provided.
    /// </summary>
    /// <param name="path">The path to the file to save. If the parent directory structure or the file does not exist, it will be created.</param>
    /// <param name="content">The string based content to save to disk.</param>
    void Save(string path, string content);
}