namespace FolderSync.Interfaces
{
    public interface IFileComparerService
    {
        bool AreEqual(string firstFile, string secondFile);
    }
}