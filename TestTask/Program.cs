using FolderSync;
using FolderSync.Providers;
using FolderSync.Repositories;
using FolderSync.Services;

var fileSystemService = new FileSystemService();
var userConsoleService = new UserConsoleService();
var userInputService = new UserInputService(userConsoleService, fileSystemService);
var fileComparer = new FileComparerService(fileSystemService, new Md5HashProvider());
var sourcePath = userInputService.GetValidPath("source");
var replicaPath = userInputService.GetValidPath("replica");
var logPath = userInputService.GetValidPath("log");
var interval = userInputService.GetInterval();

var logger = new FileLogger(fileSystemService, userConsoleService, new DateTimeProvider(), fileSystemService.Combine(logPath, "log.txt"));
var synchronizer = new FileSynchronizerService(logger, fileSystemService, fileComparer, sourcePath, replicaPath);

var app = new App(synchronizer, logger, interval);

await app.RunAsync();