using FolderSync.Interfaces;

namespace FolderSync.Services;

public class UserInputService : IUserInputService
{
    private readonly IUserConsoleService _userConsole;
    private readonly IFileSystemService _fileSystemService;

    public UserInputService(IUserConsoleService userConsole, IFileSystemService fileSystemService)
    {
        _userConsole = userConsole ?? throw new ArgumentNullException(nameof(userConsole));
        _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
    }

    public string GetValidPath(string destination)
    {
        _userConsole.WriteLine($"Enter the {destination} path.");
        while (true)
        {
            var path = _userConsole.ReadLine();
            if (_fileSystemService.PathExists(path))
            {
                return path!;
            }
            else
            {
                _userConsole.WriteLine($"The path does not exist. Enter the {destination} path again.");
            }
        }
    }

    public int GetInterval()
    {
        _userConsole.WriteLine($"Enter the interval in seconds.");
        while (true)
        {
            var value = _userConsole.ReadLine();
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            else
            {
                _userConsole.WriteLine($"Wrong value, enter the interval in seconds again.");
            }
        }
    }
}
