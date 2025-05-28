namespace FolderSync.Interfaces;

public interface IHashProvider
{
    byte[] ComputeHash(Stream stream);
}
