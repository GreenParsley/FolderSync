namespace FolderSync.Interfaces;

public interface IUserConsoleService
{
    string? ReadLine();

    void WriteLine(string message);
}