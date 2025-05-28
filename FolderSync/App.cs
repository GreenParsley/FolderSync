using FolderSync.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace FolderSync;

[ExcludeFromCodeCoverage]
public class App
{
    private readonly IFileSynchronizerService _synchronizer;
    private readonly ILogger _logger;
    private readonly int _interval;

    public App(IFileSynchronizerService synchronizer, ILogger logger, int interval)
    {
        _synchronizer = synchronizer;
        _logger = logger;
        _interval = interval;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            try
            {
                _synchronizer.Synchronize();
            }
            catch (Exception ex)
            {
                _logger.Log($"[ERROR] {ex.Message}");
            }

            await Task.Delay(_interval * 1000);
        }
    }
}
