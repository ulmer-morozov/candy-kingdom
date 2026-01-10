using System.Diagnostics;
using System.Globalization;
using System.Text.Json;

using CandyKingdom.Marcy.Utilities;

using FFmpeg.NET;
using FFmpeg.NET.Enums;

namespace CandyKingdom.Marcy.ImageTools;

public sealed class VideoManager : IVideoManager
{
    private readonly string _ffmpegPath;
    private readonly string _ffprobePath;

    public VideoManager(string? ffmpegPath = null, string? ffprobePath = null)
    {
        if (OperatingSystem.IsWindows())
        {
            _ffmpegPath = string.IsNullOrEmpty(ffmpegPath) ? "ffmpeg.exe" : ffmpegPath;
            _ffprobePath = string.IsNullOrEmpty(ffprobePath) ? "ffprobe.exe" : ffprobePath;
        }
        else
        {
            _ffmpegPath = string.IsNullOrEmpty(ffmpegPath) ? "ffmpeg" : ffmpegPath;
            _ffprobePath = string.IsNullOrEmpty(ffprobePath) ? "ffprobe" : ffprobePath;
        }

        Console.WriteLine($"Using ffmpeg path: {_ffmpegPath}");
        Console.WriteLine($"Using ffprobe path: {_ffprobePath}");
    }

    public async Task<TempVideoFile> AnalyseMp4(
      Stream sourceStream,
      CancellationToken cancellationToken = default
    )
    {
        var tempVideoFilePath = FileUtils.CreateTempFilePath("video", ".mp4");

        await using (var fileStream = new FileStream(tempVideoFilePath, FileMode.CreateNew))
        {
            await sourceStream.CopyToAsync(fileStream, cancellationToken);
            await sourceStream.DisposeAsync();
        }

        var ffprobeMeta = await GetFfprobeMetaAsync(tempVideoFilePath, cancellationToken);

        var fileFormat = GetVideoFormat(ffprobeMeta);
        var videoMeta = ToVideoMeta(ffprobeMeta);

        var tempFileVideo = new TempVideoFile
        {
            File = new FileInfo(tempVideoFilePath),
            Format = fileFormat,
            Meta = videoMeta
        };

        return tempFileVideo;
    }

    public async Task Convert(
      Stream sourceStream,
      ICollection<VideoSetup> setups,
      VideoConvertParameters parameters,
      Func<VideoSetup, TempVideoFile, Task> action,
      CancellationToken cancellationToken = default
    )
    {
        using var sourceVideo = await AnalyseMp4(sourceStream, cancellationToken);

        var ffmpeg = new Engine(_ffmpegPath);

        ffmpeg.Progress += (_, ev) => Console.WriteLine($"{ev.ProcessedDuration} of {ev.TotalDuration}");

        ffmpeg.Error += (_, ev) => throw new Exception($"{ev.Exception}\n\n{ev.Input}\n\n{ev.Output}");

        foreach (var setup in setups)
        {
            if (sourceVideo.Meta.Width < setup.Size.Width || sourceVideo.Meta.Height < setup.Size.Height)
            {
                // don't create videos for that case
                continue;
            }

            Console.WriteLine($"Converting for image setup: {setup}");

            var (left, top, cropWidth, cropHeight, finalWidth, finalHeight, needCrop) =
              MathUtils.CenterCrop(
                sourceVideo.Meta.Width,
                sourceVideo.Meta.Height,
                setup.Size.Width,
                setup.Size.Height
              );

            var inputFile = new InputFile(sourceVideo.File);

            var conversionOptions = new ConversionOptions
            {
                PixelFormat = "yuv420p",
                VideoCodecPreset = VideoCodecPreset.veryslow,
                RemoveAudio = parameters.RemoveAudio
            };

            if (setup.Format == VideoFormat.WebM)
            {
                // force vp8 fot Safari compability
                // conversionOptions.VideoCodec = VideoCodec.libvpx;
            }

            if (finalWidth % 2 != 0)
            {
                finalWidth--;
            }

            if (finalHeight % 2 != 0)
            {
                finalHeight--;
            }

            if (needCrop)
            {
                conversionOptions.ExtraArguments =
                  $"-filter:v \"crop={cropWidth}:{cropHeight}:{left}:{top},scale={finalWidth}:{finalHeight}\"";
            }
            else
            {
                conversionOptions.CustomWidth = finalWidth;
                conversionOptions.VideoSize = VideoSize.Custom;
            }

            // conversionOptions.CutMedia(TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(10));

            var outputFile = new FileInfo(
              FileUtils.CreateTempFilePath("result_video", setup.Format.Extension)
            );

            var output = new OutputFile(outputFile);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var outputData = await ffmpeg.ConvertAsync(
              inputFile,
              output,
              conversionOptions,
              cancellationToken
            );

            stopwatch.Stop();

            Console.WriteLine($"VIDEO CONVERTED IN {stopwatch.Elapsed}");

            var outputMetadata = await ffmpeg.GetMetaDataAsync(
              new InputFile(outputData.FileInfo),
              cancellationToken
            );


            // if VideoData is null it could be because InvariantGlobalization is set to true
            // issue with FFmpeg.NET

            var tempFileVideo = new TempVideoFile
            {
                File = outputMetadata.FileInfo,
                Format = GetVideoFormat(outputMetadata.VideoData.Format),
                Meta = ToVideoMeta(outputMetadata)
            };

            await action(setup, tempFileVideo);
        }
    }

    private async Task<FFProbeMeta> GetFfprobeMetaAsync(string videoFilePath, CancellationToken cancellationToken = default)
    {
        var ffprobeArgs = $"-v quiet -show_format -show_streams -print_format json \"{videoFilePath}\"";

        Console.WriteLine($"FFProbe input: {ffprobeArgs}");

        var procesStartInfo = new ProcessStartInfo(_ffprobePath, ffprobeArgs)
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        var process = Process.Start(procesStartInfo) ?? throw new Exception("Cannot start ffprobe process");

        var output = process.StandardOutput.ReadToEnd();

        var err = process.StandardError.ReadToEnd();

        if (!string.IsNullOrWhiteSpace(err))
        {
            throw new Exception(err);
        }

        await process.WaitForExitAsync(cancellationToken);

        Console.WriteLine($"FFProbe output: {output}");

        var videoMetadata = JsonSerializer.Deserialize<FFProbeMeta>(output) ?? throw new Exception($"Cannot deserialize {nameof(FFProbeMeta)} from {output}");

        return videoMetadata;
    }

    private static VideoFormat GetVideoFormat(FFProbeMeta probeMeta)
    {
        var videoSources = probeMeta.Streams.Where(x => x.CodecType == "video");
        var codecNames = videoSources.Select(x => x.CodecName).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

        var formatName = codecNames.FirstOrDefault() ?? probeMeta.Format.FormatName;

        try
        {
            return GetVideoFormat(formatName);
        }
        catch (Exception)
        {
            Console.WriteLine($"Unknown video format from ffprobe: {formatName}");
            Console.WriteLine($"Probe format name: {probeMeta.Format.FormatName}");
            Console.WriteLine($"Codec names from probe: {string.Join(", ", codecNames)}");
            throw;
        }

    }

    private static VideoFormat GetVideoFormat(string? format)
    {
        if (string.IsNullOrEmpty(format))
        {
            throw new Exception("Video format can not be null or empty");
        }

        if (format.StartsWith("h264") || format.Contains("mp4"))
        {
            return VideoFormat.Mp4;
        }

        if (format.StartsWith("vp8") || format.StartsWith("vp9"))
        {
            return VideoFormat.WebM;
        }

        throw new NotImplementedException($"Unknown format {format}");
    }


    private static VideoMeta ToVideoMeta(MetaData metadata)
    {
        var sizes = metadata.VideoData.FrameSize.Split('x').Select(int.Parse).ToArray();

        var meta = new VideoMeta
        {
            Width = sizes[0],
            Height = sizes[1],
            ByteCount = metadata.FileInfo.Length,
            FrameRate = metadata.VideoData.Fps,
            Duration = metadata.Duration,
            FullFormat = metadata.VideoData.Format,
            HasAudio = metadata.AudioData != null
        };

        return meta;
    }

    private static VideoMeta ToVideoMeta(FFProbeMeta probeMeta)
    {
        var videoSource = probeMeta.Streams.FirstOrDefault(x => x.CodecType == "video");
        var audioSource = probeMeta.Streams.FirstOrDefault(x => x.CodecType == "audio");

        if (videoSource == null)
        {
            throw new Exception($"video stream can not be null {probeMeta}");
        }

        var frameRateArr = videoSource.RFrameRate.Split('/');

        var frameRate = frameRateArr.Length switch
        {
            0 => 0,
            1 => double.Parse(frameRateArr[0], CultureInfo.InvariantCulture),
            2 => double.Parse(frameRateArr[0], CultureInfo.InvariantCulture) / double.Parse(frameRateArr[1], CultureInfo.InvariantCulture),
            _ => 0,
        };

        var size = long.Parse(probeMeta.Format.Size, CultureInfo.InvariantCulture);

        var duration = double.Parse(videoSource.Duration, CultureInfo.InvariantCulture);

        var meta = new VideoMeta
        {
            Width = videoSource.Width ?? 0,
            Height = videoSource.Height ?? 0,
            ByteCount = size,
            FrameRate = frameRate,
            Duration = TimeSpan.FromSeconds(duration),
            FullFormat = videoSource.CodecLongName ?? probeMeta.Format.FormatLongName,
            HasAudio = audioSource != null
        };

        return meta;
    }
}
