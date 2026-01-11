using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

using CandyKingdom.Marcy.Utilities;

namespace CandyKingdom.Marcy.Serialization;

public static class UseFileCacheExtensions
{
    public static Task<T?> ReadJsonFromCache<T>(
      this IUseFileCache instance,
      string prefix,
      ICollection<string> keys,
      CancellationToken cancellationToken = default
    )
    {
        var fileName = CombineToJsonName(prefix, keys);

        return ReadJsonFromCache<T>(instance, fileName, cancellationToken);
    }

    public static async Task<T?> ReadJsonFromCache<T>(
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

    public static Task StoreJsonInCache<T>(
      this IUseFileCache instance,
      string prefix,
      ICollection<string> keys,
      T obj,
      CancellationToken cancellationToken = default
    )
    {
        var fileName = CombineToJsonName(prefix, keys);

        return StoreJsonInCache(instance, fileName, obj, cancellationToken);
    }

    public static async Task StoreJsonInCache<T>(
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

    public static FileInfo? ReadFileFromCache(
      this IUseFileCache instance,
      string prefix,
      ICollection<string> keys,
      string extesion
    )
    {
        var fileName = CombineToFileName(prefix, keys, extesion);

        return ReadFileFromCache(instance, fileName);
    }

    public static FileInfo? ReadFileFromCache(
      this IUseFileCache instance,
      string fileName
    )
    {
        var filePath = Path.Combine(instance.CacheDir.FullName, fileName);
        var fileInfo = new FileInfo(filePath);

        if (!fileInfo.Exists)
        {
            return default;
        }

        return fileInfo;
    }

    public static async Task<FileInfo> StoreFileInCache(
      this IUseFileCache instance,
      string prefix,
      ICollection<string> keys,
      FileInfo sourceFile,
      CancellationToken cancellationToken = default
    )
    {
        var fileName = CombineToFileName(prefix, keys, sourceFile.Extension);

        return await StoreFileInCache(instance, fileName, sourceFile, cancellationToken);
    }

    public static async Task<FileInfo> StoreFileInCache(
      this IUseFileCache instance,
      string fileName,
      FileInfo sourceFile,
      CancellationToken cancellationToken = default
    )
    {
        var filePath = Path.Combine(instance.CacheDir.FullName, fileName);

        File.Copy(sourceFile.FullName, filePath);

        await using var sourceFileStream = new FileStream(sourceFile.FullName, FileMode.Open);
        await using var targetFileStream = new FileStream(filePath, FileMode.CreateNew);

        await sourceFileStream.CopyToAsync(targetFileStream, cancellationToken);

        return new FileInfo(filePath);
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

        var fileCacheInfo = await instance.ReadJsonFromCache<FileCacheInfo>(
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

        await instance.StoreJsonInCache(infoPrefix, keys, info, cancellationToken);

        return info;
    }

    private static string CombineToJsonName(string prefix, ICollection<string> keys)
    {
        var nameParts = keys.Prepend(prefix);
        var fileName = $"{string.Join('_', nameParts)}.json";

        return fileName;
    }

    private static string CombineToFileName(string prefix, ICollection<string> keys, string extension)
    {
        var nameParts = keys.Prepend(prefix);
        var fileName = $"{string.Join('_', nameParts)}{extension}";

        return fileName;
    }
}
