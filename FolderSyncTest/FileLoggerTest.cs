using FolderSync.Interfaces;
using FolderSync.Providers;
using FolderSync.Repositories;
using FolderSync.Services;
using NSubstitute;
using System.IO;

namespace FolderSyncTest;

public class FileLoggerTest
{
    private readonly FileLogger _cut;
    private readonly IUserConsoleService _userConsoleService;
    private readonly IFileSystemService _fileSystemService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly string _logPath = "../log/path";
    public FileLoggerTest()
    {
        _fileSystemService = Substitute.For<IFileSystemService>();
        _userConsoleService = Substitute.For<IUserConsoleService>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        _cut = new FileLogger(_fileSystemService, _userConsoleService, _dateTimeProvider, _logPath);
    }

    [Fact]
    public void Log_AppendLogs_IncludeMessage()
    {
        //Arrange
        var message = "message";
        var dateTime = DateTime.Now;
        var logMessage = $"[{dateTime:yyyy-MM-dd HH:mm:ss}] {message}";
        _dateTimeProvider.Now.Returns(dateTime);

        //Act
        _cut.Log(message);

        //Assert
        _fileSystemService.Received(1).WriteLine(logMessage);
        _fileSystemService.Received(1).AppendAllText(_logPath, logMessage + Environment.NewLine);
    }
}