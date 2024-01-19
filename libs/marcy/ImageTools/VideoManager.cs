using System.Diagnostics;
using CandyKingdom.Marcy.Utilities;
using FFmpeg.NET;
using FFmpeg.NET.Enums;

namespace CandyKingdom.Marcy.ImageTools;

public sealed class VideoManager : IVideoManager
{
    private readonly string? _ffmpegPath = null;

    public async Task<TempVideoFile> AnalyseMp4(
      Stream sourceStream,
      CancellationToken cancellationToken = default
    )
    {
        var tempVideoFilePath = FileUtils.CreateTempFilePath("video", ".some-video");

        await using (var fileStream = new FileStream(tempVideoFilePath, FileMode.CreateNew))
        {
            await sourceStream.CopyToAsync(fileStream, cancellationToken);
            await sourceStream.DisposeAsync();
        }

        var ffmpeg = new Engine(_ffmpegPath);
        var inputFile = new InputFile(tempVideoFilePath);

        var metadata = await ffmpeg.GetMetaDataAsync(inputFile, cancellationToken);

        var tempFileVideo = new TempVideoFile
        {
            File = new FileInfo(tempVideoFilePath),
            Format = GetVideoFormat(metadata.VideoData.Format),
            Meta = ToVideoMeta(metadata)
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

        ffmpeg.Progress += (_, ev) =>
        {
            Console.WriteLine($"{ev.ProcessedDuration} of {ev.TotalDuration}");
        };

        ffmpeg.Error += (_, ev) =>
        {
            throw new Exception($"{ev.Exception}\n\n{ev.Input}\n\n{ev.Output}");
        };

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
                finalWidth--;

            if (finalHeight % 2 != 0)
                finalHeight--;

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

            var tempFileVideo = new TempVideoFile
            {
                File = outputMetadata.FileInfo,
                Format = GetVideoFormat(outputMetadata.VideoData.Format),
                Meta = ToVideoMeta(outputMetadata)
            };

            await action(setup, tempFileVideo);
        }
    }

    private static VideoFormat GetVideoFormat(string format)
    {
        if (format.StartsWith("h264"))
            return VideoFormat.Mp4;

        if (format.StartsWith("vp8") || format.StartsWith("vp9"))
            return VideoFormat.WebM;

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
}
