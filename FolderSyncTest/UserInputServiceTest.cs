using FolderSync.Interfaces;
using FolderSync.Services;
using NSubstitute;

namespace FolderSyncTest;

public class UserInputServiceTest
{
    private readonly UserInputService _cut;
    private readonly IUserConsoleService _userConsole;
    private readonly IFileSystemService _fileSystemService;
    public UserInputServiceTest()
    {
        _userConsole = Substitute.For<IUserConsoleService>();
        _fileSystemService = Substitute.For<IFileSystemService>();
        _cut = new UserInputService(_userConsole, _fileSystemService);
    }

    [Fact]
    public void GetValidPath_ReturnsValidPath_WhenUserEntersExistingPath()
    {
        //Arrange
        var path = "../valid/path";
        _userConsole.ReadLine().Returns(path);
        _fileSystemService.PathExists(path).Returns(true);

        //Act
        var result = _cut.GetValidPath("source");

        //Assert
        result.Equals(path);
    }

    [Fact]
    public void GetValidPath_ReturnsValidPath_OnlyWhenUserEntersExistingPath()
    {
        //Arrange
        var path = "../valid/path";
        var notValidPath = "../not/valid/path";
        _userConsole.ReadLine().Returns(notValidPath, path);
        _fileSystemService.PathExists(notValidPath).Returns(false);
        _fileSystemService.PathExists(path).Returns(true);

        //Act
        var result = _cut.GetValidPath("source");

        //Assert
        result.Equals(path);
    }

    [Fact]
    public void GetInterval_ReturnsInterval_WhenUserEntersCorrectValue()
    {
        //Arrange
        var interval = "10";
        _userConsole.ReadLine().Returns(interval);

        //Act
        var result = _cut.GetInterval();

        //Assert
        result.Equals(interval);
    }

    [Theory]
    [InlineData("1.2")]
    [InlineData("1.2f")]
    [InlineData("")]
    [InlineData("not valid value")]
    [InlineData(null)]
    public void GetInterval_ReturnsInterval_OnlyWhenUserEntersCorrectValue(string notValidValue)
    {
        //Arrange
        var validValue = "10";
        _userConsole.ReadLine().Returns(notValidValue, validValue);

        //Act
        var result = _cut.GetInterval();

        //Assert
        result.Equals(validValue);
    }
}