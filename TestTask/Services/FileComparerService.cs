using FolderSync.Interfaces;

namespace FolderSync.Services;

public class FileComparerService : IFileComparerService
{
    private readonly IFileSystemService _fileSystemService;
    private readonly IHashProvider _hashProvider;

    public FileComparerService(IFileSystemService fileSystemService, IHashProvider hashProvider)
    {
        _fileSystemService = fileSystemService;
        _hashProvider = hashProvider;
    }
    public bool AreEqual(string firstFile, string secondFile)
    {
        using var firstFileStream = _fileSystemService.OpenRead(firstFile);
        using var secondFileStream = _fileSystemService.OpenRead(secondFile);

        var firstHash = _hashProvider.ComputeHash(firstFileStream!);
        var secondHash = _hashProvider.ComputeHash(secondFileStream!);

        return firstHash.SequenceEqual(secondHash);
    }
}
