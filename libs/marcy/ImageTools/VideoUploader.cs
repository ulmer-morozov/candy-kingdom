using System.Collections.Immutable;
using System.Diagnostics;
using CandyKingdom.Marcy.Storage;
using CandyKingdom.Marcy.Utilities;

namespace CandyKingdom.Marcy.ImageTools;

public sealed class VideoUploader : MediaUploderBase, IVideoUploader
{
    private readonly IVideoManager _videoManager;
    protected override VideoUploaderConfig Config { get; }

    public VideoUploader(IFileStorage fileStorage, IVideoManager videoManager, VideoUploaderConfig config)
        : base(fileStorage)
    {
        _videoManager = videoManager;
        Config = config;
    }

    public async Task<ImmutableDictionary<VideoSetup, FileSrc<VideoMeta>>> ConvertAndStore(
     MemoryStream value,
     ImmutableList<VideoSetup> setups,
     VideoConvertParameters convertParameters,
     Func<VideoSetup, TempVideoFile, FileSrc<VideoMeta>, Task>? action = null,
     CancellationToken cancellationToken = default
   )
    {
        var dict = new Dictionary<VideoSetup, FileSrc<VideoMeta>>();

        async Task OnVideo(VideoSetup setup, TempVideoFile videoFile)
        {
            StoredFile storedFile;

            using (
              var fileStream = new FileStream(videoFile.File.FullName, FileMode.Open, FileAccess.Read)
            )
            {
                var hash = await fileStream.CalcMd5AsBase62Async(cancellationToken);

                var fileName =
                  $"{videoFile.Meta.Width}x{videoFile.Meta.Height}_{hash}{videoFile.Format.Extension}";

                Trace.WriteLine($"On video store: {fileName}");

                fileStream.Seek(0, SeekOrigin.Begin);

                storedFile = await _fileStorage.Store(
                  stream: fileStream,
                  name: fileName,
                  mimeType: videoFile.Format.MimeType,
                  cancellationToken
                );
            }

            Trace.WriteLine($"On video stored: {storedFile.Url}");

            var fileSrc = new FileSrc<VideoMeta>
            {
                Url = storedFile.Url,
                Meta = videoFile.Meta,
                MimeType = videoFile.Format.MimeType
            };

            dict[setup] = fileSrc;

            if (action != null)
            {
                await action(setup, videoFile, fileSrc);
            }

            videoFile.Dispose();
        }

        await _videoManager.Convert(value, setups, convertParameters, OnVideo, cancellationToken);

        return dict.ToImmutableDictionary();
    }

}
