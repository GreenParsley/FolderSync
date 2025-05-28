using FolderSync.Interfaces;
using FolderSync.Services;
using NSubstitute;
using System.Diagnostics;

namespace FolderSyncTest;

public class FileSynchronizerServiceTest
{
    private readonly FileSynchronizerService _cut;
    private readonly ILogger _logger;
    private readonly IFileSystemService _fileSystemService;
    private readonly IFileComparerService _fileComparer;
    private readonly string _sourcePath = "../source/path";
    private readonly string _replicaPath = "../replica/path";

    public FileSynchronizerServiceTest()
    {
        _logger = Substitute.For<ILogger>();
        _fileSystemService = Substitute.For<IFileSystemService>();
        _fileComparer = Substitute.For<IFileComparerService>();
        _cut = new FileSynchronizerService(_logger, _fileSystemService, _fileComparer, _sourcePath, _replicaPath);
    }

    [Fact]
    public void Synchronize_CreateFolder_WhenNotExist()
    {
        //Arrange
        var firstDirectory = "directory1";
        var directories = new List<string>()
        {
            $".{_sourcePath}/{firstDirectory}"
        };
        var destinationDirectory = $".{_replicaPath}/{firstDirectory}";
        _fileSystemService.GetDirectories(_sourcePath).Returns(directories, new List<string>());
        _fileSystemService.GetRelativePath(_sourcePath, directories[0]).Returns(firstDirectory);
        _fileSystemService.Combine(_replicaPath, firstDirectory).Returns(destinationDirectory);
        _fileSystemService.DirectoryExists(destinationDirectory).Returns(false);

        _fileSystemService.GetFiles(_sourcePath).Returns(new List<string>(), new List<string>());

        //Act
        _cut.Synchronize();

        //Assert
        _fileSystemService.Received(1).CreateDirectory(destinationDirectory);
    }

    [Fact]
    public void Synchronize_CreateFiles_WhenNotExist()
    {
        //Arrange
        var firstFile = "file1";
        var files = new List<string>()
        {
            $".{_sourcePath}/{firstFile}"
        };
        var destinationFile = $".{_replicaPath}/{firstFile}";
        _fileSystemService.GetFiles(_sourcePath).Returns(files);
        _fileSystemService.GetRelativePath(_sourcePath, files[0]).Returns(firstFile);
        _fileSystemService.Combine(_replicaPath, firstFile).Returns(destinationFile);
        _fileSystemService.FileExists(destinationFile).Returns(false);

        //Act
        _cut.Synchronize();

        //Assert
        _fileSystemService.Received(1).CopyFile(files[0], destinationFile, false);
    }

    [Fact]
    public void Synchronize_UpdateFiles_WhenNewVersionExists()
    {
        //Arrange
        var firstFile = "file1";
        var files = new List<string>()
        {
            $".{_sourcePath}/{firstFile}"
        };
        var destinationFile = $".{_replicaPath}/{firstFile}";
        _fileSystemService.GetFiles(_sourcePath).Returns(files);
        _fileSystemService.GetRelativePath(_sourcePath, files[0]).Returns(firstFile);
        _fileSystemService.Combine(_replicaPath, firstFile).Returns(destinationFile);
        _fileSystemService.FileExists(destinationFile).Returns(true);
        _fileComparer.AreEqual(files[0], destinationFile).Returns(false);

        //Act
        _cut.Synchronize();

        //Assert
        _fileSystemService.Received(1).CopyFile(files[0], destinationFile, true);
    }

    [Fact]
    public void Synchronize_RemoveFiles_WhenNotExist()
    {
        //Arrange
        var firstFile = "file1";
        var files = new List<string>()
        {
            $".{_replicaPath}/{firstFile}"
        };
        var sourceFile = $".{_sourcePath}/{firstFile}";
        _fileSystemService.GetFiles(_replicaPath).Returns(files);
        _fileSystemService.GetRelativePath(_replicaPath, files[0]).Returns(firstFile);
        _fileSystemService.Combine(_sourcePath, firstFile).Returns(sourceFile);
        _fileSystemService.FileExists(sourceFile).Returns(false);

        //Act
        _cut.Synchronize();

        //Assert
        _fileSystemService.Received(1).DeleteFile(files[0]);
    }

    [Fact]
    public void Synchronize_RemoveFolders_WhenNotExist()
    {
        //Arrange
        var firstDirectory = "directory1";
        var directories = new List<string>()
        {
            $".{_replicaPath}/{firstDirectory}"
        };
        var sourceDirectory = $".{_sourcePath}/{firstDirectory}";
        _fileSystemService.GetDirectories(_replicaPath).Returns(directories);
        _fileSystemService.GetRelativePath(_replicaPath, directories[0]).Returns(firstDirectory);
        _fileSystemService.Combine(_sourcePath, firstDirectory).Returns(sourceDirectory);
        _fileSystemService.DirectoryExists(sourceDirectory).Returns(false);

        //Act
        _cut.Synchronize();

        //Assert
        _fileSystemService.Received(1).DeleteDirectory(directories[0], true);
    }
}