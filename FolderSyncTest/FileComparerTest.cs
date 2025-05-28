using FolderSync.Interfaces;
using FolderSync.Services;
using NSubstitute;

namespace FolderSyncTest;

public class FileComparerTest
{
    private readonly FileComparerService _cut;
    private readonly IHashProvider _hashProvider;
    private readonly IFileSystemService _fileSystemService;
    public FileComparerTest()
    {
        _hashProvider = Substitute.For<IHashProvider>();
        _fileSystemService = Substitute.For<IFileSystemService>();
        _cut = new FileComparerService(_fileSystemService, _hashProvider);
    }

    [Fact]
    public void AreEqual_ReturnsTrue_WhenHashesAreEqual()
    {
        // Arrange
        var firstStream = new MemoryStream();
        var secondStream = new MemoryStream();
        var hash = new byte[] { 1, 2, 3, 4 };

        _fileSystemService.OpenRead("file1.txt").Returns(firstStream);
        _fileSystemService.OpenRead("file2.txt").Returns(secondStream);

        _hashProvider.ComputeHash(firstStream).Returns(hash);
        _hashProvider.ComputeHash(secondStream).Returns(hash);

        // Act
        var result = _cut.AreEqual("file1.txt", "file2.txt");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AreEqual_ReturnsFalse_WhenHashesAreDifferent()
    {
        // Arrange
        var firstStream = new MemoryStream();
        var secondStream = new MemoryStream();
        var firstHash = new byte[] { 1, 2, 3, 4 };
        var secondHash = new byte[] { 5, 6, 7, 8 };

        _fileSystemService.OpenRead("file1.txt").Returns(firstStream);
        _fileSystemService.OpenRead("file2.txt").Returns(secondStream);

        _hashProvider.ComputeHash(firstStream).Returns(firstHash);
        _hashProvider.ComputeHash(secondStream).Returns(secondHash);

        // Act
        var result = _cut.AreEqual("file1.txt", "file2.txt");

        // Assert
        Assert.False(result);
    }
}