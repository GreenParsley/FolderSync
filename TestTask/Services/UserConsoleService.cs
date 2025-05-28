using FolderSync.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace FolderSync.Services;

[ExcludeFromCodeCoverage]
public class UserConsoleService : IUserConsoleService
{
    public string? ReadLine() => Console.ReadLine();

    public void WriteLine(string message) => Console.WriteLine(message);
}
