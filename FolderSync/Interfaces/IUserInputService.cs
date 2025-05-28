namespace FolderSync.Interfaces;

public interface IUserInputService
{
    int GetInterval();

    string GetValidPath(string destination);
}