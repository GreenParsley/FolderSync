using FolderSync.Interfaces;

namespace FolderSync.Services;

public class FileSynchronizerService : IFileSynchronizerService
{
    private readonly ILogger _logger;
    private readonly IFileSystemService _fileSystemService;
    private readonly IFileComparerService _fileComparer;
    private readonly string _sourcePath;
    private readonly string _replicaPath;

    public FileSynchronizerService(ILogger logger,
        IFileSystemService fileSystemService,
        IFileComparerService fileComparer,
        string sourcePath,
        string replicaPath)
    {
        _logger = logger;
        _fileSystemService = fileSystemService;
        _fileComparer = fileComparer;
        _sourcePath = sourcePath;
        _replicaPath = replicaPath;
    }

    public void Synchronize()
    {
        CreateFolders();
        CreateOrUpdateFiles();
        RemoveFiles();
        RemoveFolders();
    }

    private void CreateFolders()
    {
        foreach (var sourceDirectory in _fileSystemService.GetDirectories(_sourcePath))
        {
            var relative = _fileSystemService.GetRelativePath(_sourcePath, sourceDirectory);
            var destinationDirectory = _fileSystemService.Combine(_replicaPath, relative);

            if (!_fileSystemService.DirectoryExists(destinationDirectory))
            {
                _fileSystemService.CreateDirectory(destinationDirectory);
                _logger.Log($"[CREATED FOLDER] {destinationDirectory}");
            }
        }
    }

    private void CreateOrUpdateFiles()
    {
        foreach (var sourceFile in _fileSystemService.GetFiles(_sourcePath))
        {
            var relative = _fileSystemService.GetRelativePath(_sourcePath, sourceFile);
            var destinationFile = _fileSystemService.Combine(_replicaPath, relative);

            if (!_fileSystemService.FileExists(destinationFile))
            {
                _fileSystemService.CopyFile(sourceFile, destinationFile, false);
                _logger.Log($"[CREATED] {destinationFile}");
            }
            else if (!_fileComparer.AreEqual(sourceFile, destinationFile))
            {
                _fileSystemService.CopyFile(sourceFile, destinationFile, true);
                _logger.Log($"[COPIED] {destinationFile}");
            }
        }
    }

    private void RemoveFiles()
    {
        foreach (var replicaFile in _fileSystemService.GetFiles(_replicaPath))
        {
            var relative = _fileSystemService.GetRelativePath(_replicaPath, replicaFile);
            var sourceFile = _fileSystemService.Combine(_sourcePath, relative);

            if (!_fileSystemService.FileExists(sourceFile))
            {
                _fileSystemService.DeleteFile(replicaFile);
                _logger.Log($"[DELETED] {replicaFile}");
            }
        }
    }

    private void RemoveFolders()
    {
        foreach (var replicaDirectory in _fileSystemService.GetDirectories(_replicaPath))
        {
            var relative = _fileSystemService.GetRelativePath(_replicaPath, replicaDirectory);
            var sourceDirectory = _fileSystemService.Combine(_sourcePath, relative);

            if (!_fileSystemService.DirectoryExists(sourceDirectory))
            {
                _fileSystemService.DeleteDirectory(replicaDirectory, true);
                _logger.Log($"[DELETED FOLDER] {replicaDirectory}");
            }
        }
    }
}
