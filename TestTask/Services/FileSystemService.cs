using FolderSync.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace FolderSync.Services;

[ExcludeFromCodeCoverage]
public class FileSystemService : IFileSystemService
{
    public bool PathExists(string? path) => Path.Exists(path);

    public IEnumerable<string> GetDirectories(string path) =>
        Directory.GetDirectories(path, "*", SearchOption.AllDirectories)
            .OrderByDescending(d => d.Length);

    public IEnumerable<string> GetFiles(string path) =>
        Directory.GetFiles(path, "*", SearchOption.AllDirectories);

    public string GetRelativePath(string basePath, string path) =>
        Path.GetRelativePath(basePath, path);

    public string Combine(string firstPath, string secondPath) =>
        Path.Combine(firstPath, secondPath);

    public string? GetDirectoryName(string path) =>
        Path.GetDirectoryName(path);

    public bool DirectoryExists(string path) =>
        Directory.Exists(path);

    public bool FileExists(string path) =>
        File.Exists(path);

    public void CreateDirectory(string path) =>
        Directory.CreateDirectory(path);

    public void CopyFile(string source, string destination, bool overwrite) =>
        File.Copy(source, destination, overwrite);

    public void DeleteFile(string path) =>
        File.Delete(path);

    public void DeleteDirectory(string path, bool recursive) =>
        Directory.Delete(path, recursive);

    public void AppendAllText(string path, string message) => 
        File.AppendAllText(path, message + Environment.NewLine);

    public void WriteLine(string message) => Console.WriteLine(message);

    public Stream? OpenRead(string file) => File.OpenRead(file);
}