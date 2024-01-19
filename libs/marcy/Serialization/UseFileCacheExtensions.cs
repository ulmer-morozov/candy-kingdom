using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

using CandyKingdom.Marcy.Utilities;

namespace CandyKingdom.Marcy.Serialization;

public static class UseFileCacheExtensions
{
    public static Task<T?> ReadFromCache<T>(
      this IUseFileCache instance,
      string prefix,
      ICollection<string> keys,
      CancellationToken cancellationToken = default
    )
    {
        var fileName = CombineToJsonName(prefix, keys);

        return ReadFromCache<T>(instance, fileName, cancellationToken);
    }

    public static async Task<T?> ReadFromCache<T>(
      this IUseFileCache instance,
      string fileName,
      CancellationToken cancellationToken = default
    )
    {
        var filePath = Path.Combine(instance.CacheDir.FullName, fileName);

        if (!File.Exists(filePath))
        {
            return default;
        }

        var jsonData = await File.ReadAllTextAsync(filePath, cancellationToken);
        var data = JsonSerializer.Deserialize<T>(jsonData) ?? throw new Exception($"Cannot deserialize {nameof(T)} from {filePath}");

        return data;
    }

    public static Task StoreInCache<T>(
      this IUseFileCache instance,
      string prefix,
      ICollection<string> keys,
      T obj,
      CancellationToken cancellationToken = default
    )
    {
        var fileName = CombineToJsonName(prefix, keys);

        return StoreInCache(instance, fileName, obj, cancellationToken);
    }

    public static async Task StoreInCache<T>(
      this IUseFileCache instance,
      string fileName,
      T obj,
      CancellationToken cancellationToken = default
    )
    {
        var filePath = Path.Combine(instance.CacheDir.FullName, fileName);
        var json = JsonSerializer.Serialize(obj);

        await File.WriteAllTextAsync(filePath, json, cancellationToken);
    }

    [return: NotNull]
    public static async Task<FileCacheInfo> GetOrCreateFileCacheInfo(
      this IUseFileCache instance,
      FileInfo file,
      DefferedStreamClone streamClone,
      CancellationToken cancellationToken = default
    )
    {
        const string infoPrefix = "info";

        var fileId = $"{file.FullName}|{file.Length}|{file.LastWriteTimeUtc}".CalcMd5AsBase62();

        var keys = ImmutableList.Create(fileId);

        var fileCacheInfo = await instance.ReadFromCache<FileCacheInfo>(
          infoPrefix,
          keys,
          cancellationToken
        );

        if (fileCacheInfo != null)
        {
            return fileCacheInfo;
        }

        var imageHash = await streamClone.Value.CalcMd5AsBase62Async(cancellationToken);

        streamClone.Value.Seek(0, SeekOrigin.Begin);

        var info = new FileCacheInfo { Hash = imageHash };

        await instance.StoreInCache(infoPrefix, keys, info, cancellationToken);

        return info;
    }

    private static string CombineToJsonName(string prefix, ICollection<string> keys)
    {
        var nameParts = keys.Prepend(prefix);
        var fileName = $"{string.Join('_', nameParts)}.json";

        return fileName;
    }
}
