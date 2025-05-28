using FolderSync.Interfaces;
using FolderSync.Providers;

namespace FolderSync.Repositories;

public class FileLogger : ILogger
{
    private readonly IFileSystemService _fileSystemService;
    private readonly IUserConsoleService _userConsoleService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly string _logPath;

    public FileLogger(IFileSystemService fileSystemService, IUserConsoleService userConsoleService, IDateTimeProvider dateTimeProvider, string logPath)
    {
        _fileSystemService = fileSystemService;
        _userConsoleService = userConsoleService;
        _dateTimeProvider = dateTimeProvider;
        _logPath = logPath;
    }

    public void Log(string message)
    {
        var logMessage = $"[{_dateTimeProvider.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        _fileSystemService.WriteLine(logMessage);
        _fileSystemService.AppendAllText(_logPath, logMessage + Environment.NewLine);
    }
}
