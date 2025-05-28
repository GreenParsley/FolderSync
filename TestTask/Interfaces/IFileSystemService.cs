namespace FolderSync.Interfaces;

public interface IFileSystemService
{
    bool PathExists(string? path);

    IEnumerable<string> GetDirectories(string path);

    IEnumerable<string> GetFiles(string path);

    string GetRelativePath(string basePath, string path);

    string Combine(string firstPath, string secondPath);

    string? GetDirectoryName(string path);

    bool DirectoryExists(string path);

    bool FileExists(string path);

    void CreateDirectory(string path);

    void CopyFile(string source, string destination, bool overwrite);

    void DeleteFile(string path);

    void DeleteDirectory(string path, bool recursive);

    void AppendAllText(string path, string message);

    void WriteLine(string message);

    Stream? OpenRead(string file);
}