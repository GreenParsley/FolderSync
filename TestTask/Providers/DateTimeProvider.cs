using FolderSync.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace FolderSync.Providers;

[ExcludeFromCodeCoverage]
public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
}
