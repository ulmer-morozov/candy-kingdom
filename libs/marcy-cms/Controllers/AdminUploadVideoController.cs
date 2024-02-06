using System.Collections.Immutable;
using System.Drawing;

using CandyKingdom.Marcy;
using CandyKingdom.Marcy.ImageTools;
using CandyKingdom.Marcy.Immutables;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CandyKingdom.MarcyCms.Sample.Controllers.Admin;

[Authorize]
[Route("Api/Admin/Upload/Video")]
public sealed class AdminUploadVideoController : ControllerBase
{
    public static readonly ImmutableList<VideoSetup> DefaultSetups = GenerateVideoSetups();

    private readonly IVideoUploader _videoUploader;

    public AdminUploadVideoController(IVideoUploader videoUploader)
    {
        _videoUploader = videoUploader;
    }

    [HttpPost("Raw")]
    [RequestSizeLimit(100_000_000)]
    public async Task<Results<BadRequest<string>, Ok<FileSrc<VideoMeta>>>> UploadRawVideo([FromForm] IFormFile file, CancellationToken cancellationToken = default)
    {
        if (file == null)
        {
            return TypedResults.BadRequest("Field \"file\": cannot be empty");
        }

        if (file.Length <= 0)
        {
            return TypedResults.BadRequest("Field \"file\": stream cannot have zero length");
        }

        var videoStream = file.OpenReadStream();

        var uploadedImageSrc = await _videoUploader.ConvertAndStore
        (
            stream: videoStream,
            setup: VideoSetup.Empty,
            convertParameters: VideoConvertParameters.Default,
            cancellationToken: cancellationToken
        );

        return TypedResults.Ok(uploadedImageSrc);
    }

    [HttpPost]
    [RequestSizeLimit(100_000_000)]
    public async Task<Results<BadRequest<string>, Ok<FileSrc<VideoMeta>>>> UploadSingleVideo([FromForm] IFormFile file, [FromForm] int width = 0, [FromForm] int height = 0, string format = "", CancellationToken cancellationToken = default)
    {
        if (file == null)
        {
            return TypedResults.BadRequest("Field \"file\": cannot be empty");
        }

        if (file.Length <= 0)
        {
            return TypedResults.BadRequest("Field \"file\": stream cannot have zero length");
        }

        var videoStream = file.OpenReadStream();

        var videoSetup = new VideoSetup
        {
            Size = new Size(width, height),
            Format = GetFormatByExtension(string.IsNullOrWhiteSpace(format) ? file.Name : format)
        };

        var uploadedVideoSrc = await _videoUploader.ConvertAndStore
        (
            stream: videoStream,
            setup: VideoSetup.Empty,
            convertParameters: VideoConvertParameters.Default,
            cancellationToken: cancellationToken
        );

        return TypedResults.Ok(uploadedVideoSrc);
    }

    [HttpPost("Complex")]
    [RequestSizeLimit(100_000_000)]
    public async Task<Results<BadRequest<string>, Ok<Video>>> UploadVideo([FromForm] IFormFile file, bool removeAudio = false, CancellationToken cancellationToken = default)
    {
        if (file == null)
        {
            return TypedResults.BadRequest("Field \"file\": cannot be empty");
        }

        if (file.Length <= 0)
        {
            return TypedResults.BadRequest("Field \"file\": stream cannot have zero length");
        }

        var videoStream = file.OpenReadStream();

        var convertParameters = new VideoConvertParameters
        {
            RemoveAudio = removeAudio
        };

        var uploadedVideoSrcDict = await _videoUploader.ConvertAndStore
        (
            stream: videoStream,
            setups: DefaultSetups,
            convertParameters: convertParameters,
            cancellationToken: cancellationToken
        );

        var video = new Video
        (
           new[] { new VideoSource(uploadedVideoSrcDict.Values.ToImmutableList2()) }
        );

        return TypedResults.Ok(video);
    }

    private static VideoFormat GetFormatByExtension(string fileName)
    {
        var extenision = Path.GetExtension(fileName).ToLowerInvariant();

        return extenision switch
        {
            ".mp4" => VideoFormat.Mp4,
            ".webm" => VideoFormat.WebM,
            _ => VideoFormat.Empty,
        };
    }

    private static ImmutableList<VideoSetup> GenerateVideoSetups() // todo: move to cms cofigutation
    {
        var formats = ImmutableList.Create(VideoFormat.WebM, VideoFormat.Mp4);
        var widths = ImmutableList.Create(480, 640, 960, 1920);

        var setups = formats
            .SelectMany(
                f =>
                    widths.Select(
                        w => new VideoSetup { Format = f, Size = new Size(w, 0) }
                    )
            )
            .ToImmutableList();

        return setups;
    }
}
