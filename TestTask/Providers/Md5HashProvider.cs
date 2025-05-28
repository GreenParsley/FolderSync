using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using FolderSync.Interfaces;

namespace FolderSync.Providers;

[ExcludeFromCodeCoverage]
public class Md5HashProvider : IHashProvider
{
    public byte[] ComputeHash(Stream stream)
    {
        using var md5 = MD5.Create();
        return md5.ComputeHash(stream);
    }
}
