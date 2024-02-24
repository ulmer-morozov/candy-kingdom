using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CandyKingdom.Marcy.Utilities;

public sealed class BinWrapper
{
    public const string DefaultDestination = "bin-wrapper-downloads";

    public string Destination { get; }

    public ImmutableList<OsDependendName> ExecutableNames { get; }
    public ImmutableList<OsDependendSource> Sources { get; }

    private readonly object _lock = new();
    private static readonly HttpClient HttpClient = new();
    private Task? _downloadTask;

    public BinWrapper(
      IEnumerable<OsDependendName> executableNames,
      IEnumerable<OsDependendSource> sources,
      string? destination = null
    )
    {
        Destination = Path.GetFullPath(destination?.Trim() ?? DefaultDestination);

        Sources =
          sources as ImmutableList<OsDependendSource>
          ?? sources?.ToImmutableList()
          ?? [];

        ExecutableNames =
          executableNames as ImmutableList<OsDependendName>
          ?? executableNames?.ToImmutableList()
          ?? [];

        if (Sources.IsEmpty)
        {
            throw new ArgumentException("Не может быть пустым", nameof(sources));
        }

        if (ExecutableNames.IsEmpty)
        {
            throw new ArgumentException("Не может быть пустым", nameof(executableNames));
        }
    }

    public Task Run(IEnumerable<string> args, CancellationToken cancellationToken = default)
    {
        return Run(cancellationToken, args.ToArray());
    }

    public async Task Run(CancellationToken cancellationToken = default, params string[] args)
    {
        var executableFile = GetExecutable();

        if (!executableFile.Exists)
        {
            lock (_lock)
            {
                if (_downloadTask == null)
                {

                    var name = executableFile.Name;

                    Console.WriteLine($"CALLING NEW DOWNLOAD {executableFile.Name}");

                    async Task Download()
                    {
                        await DownloadExecutable();
                        Thread.Sleep(1000);
                    }

                    _downloadTask = Download();
                }
                else
                {
                    var elsd = 12;
                }
            }

            await _downloadTask;
        }

        var argumentString = string.Join(" ", args);

        Process? process = null;

        try
        {
            lock (_lock)
            {
                process = new Process
                {
                    StartInfo =
                    {
                        FileName = executableFile.FullName,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        Arguments = argumentString
                    }
                };

                process.Start();
                process.WaitForExit();
            }

        }
        catch (Exception e)
        {
            process?.Dispose();
        }
    }

    public Task DownloadExecutable()
    {
        return DownloadExecutable(CurrentPlatform, Is64Bit);
    }

    public async Task DownloadExecutable(OSPlatform platform, bool x64)
    {
        var executableFile = GetExecutable(platform, x64);

        // создадим директорию
        if (executableFile.Directory != null && !executableFile.Directory.Exists)
        {
            executableFile.Directory.Create();
        }

        var relatedSources = GetSources(platform, x64);

        foreach (var relatedSource in relatedSources)
        {
            await Download(relatedSource);
        }
    }

    private async Task Download(OsDependendSource source)
    {
        var filePath = Path.Combine(Destination, source.FileName);

        Console.WriteLine($"Downloading {source.FileName} > {source.Url} to {filePath}");

        var fileBytes = await HttpClient.GetByteArrayAsync(source.Url);

        File.WriteAllBytes(filePath, fileBytes);

        Console.WriteLine($"Downloaded {source.FileName} > {source.Url} to {filePath}");

        Chmod(755, filePath);
    }

    public FileInfo GetExecutable()
    {
        return GetExecutable(CurrentPlatform, Is64Bit);
    }

    public FileInfo GetExecutable(OSPlatform platform, bool x64)
    {
        var executableName = GetExecutableName(platform, x64);
        var executableFile = new FileInfo(Path.Combine(Destination, executableName));

        return executableFile;
    }

    public static bool Is64Bit => Environment.Is64BitOperatingSystem;

    public static OSPlatform CurrentPlatform
    {
        get
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return OSPlatform.Windows;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return OSPlatform.OSX;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return OSPlatform.Linux;
            }

            // if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            //     return OSPlatform.FreeBSD;

            throw new Exception("Unknown OSPlatform");
        }
    }

    private ImmutableList<OsDependendSource> GetSources(OSPlatform platform, bool x64)
    {
        var validSources = Sources
          .Where(x => x.Platform == platform && x.x64 == x64)
          .ToImmutableList();

        if (validSources.IsEmpty)
        {
            throw new Exception($"Для платформы {platform} x64={x64} Не найдено {nameof(Sources)}");
        }

        return validSources;
    }

    private string GetExecutableName(OSPlatform platform, bool x64)
    {
        var validNames = ExecutableNames
          .Where(x => x.Platform == platform && x.x64 == x64)
          .ToImmutableList();

        if (validNames.IsEmpty)
        {
            throw new Exception(
              $"Для платформы {platform} x64={x64} Не найдено {nameof(ExecutableNames)}"
            );
        }

        if (validNames.Count > 1)
        {
            throw new Exception(
              $"Для платформы {platform} x64={x64} Найдено несколько {nameof(ExecutableNames)}: {string.Join(", ", ExecutableNames.Select(x => x.Name))}"
            );
        }

        var osDependendName = validNames.Single();
        return osDependendName.Name;
    }

    private static void Chmod(uint flag, string file)
    {
        if (CurrentPlatform == OSPlatform.Windows)
        {
            return;
        }

        Exec($"chmod {flag} \"{file}\"");
    }

    private static void Exec(string cmd)
    {
        var escapedArgs = cmd.Replace("\"", "\\\"");

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "/bin/bash",
                Arguments = $"-c \"{escapedArgs}\""
            }
        };

        process.Start();
        process.WaitForExit();
    }
}
