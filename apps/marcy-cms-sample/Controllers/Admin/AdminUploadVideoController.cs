using System.Collections.Immutable;
using System.Drawing;
using CandyKingdom.Marcy;
using CandyKingdom.Marcy.ImageTools;
using CandyKingdom.MarcyCms.Sample.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CandyKingdom.MarcyCms.Sample.Controllers.Admin;

[Authorize(Policy = Roles.Admin)]
[Route("Api/Admin/Upload/Video")]
public class AdminUploadVideoController : Controller
{
    public static readonly ImmutableList<VideoSetup> DefaultSetups = GenerateVideoSetups();

    private readonly IVideoUploader _videoUploader;

    public AdminUploadVideoController(IVideoUploader videoUploader)
    {
        _videoUploader = videoUploader;
    }

    [HttpPost("Raw")]
    [RequestSizeLimit(100_000_000)]
    public async Task<ActionResult<FileSrc<ImageMeta>>> UploadRawVideo([FromForm] IFormFile file, CancellationToken cancellationToken = default)
    {
        if (file == null)
            return BadRequest("Field \"file\": cannot be empty");

        if (file.Length <= 0)
            return BadRequest("Field \"file\": stream cannot have zero length");

        var videoStream = file.OpenReadStream();

        var uploadedImageSrc = await _videoUploader.ConvertAndStore
        (
            stream: videoStream,
            setup: VideoSetup.Empty,
            convertParameters: VideoConvertParameters.Default,
           cancellationToken: cancellationToken
        );

        return Ok(uploadedImageSrc);
    }

    [HttpPost]
    [RequestSizeLimit(100_000_000)]
    public async Task<ActionResult<FileSrc<ImageMeta>>> UploadSingleVideo([FromForm] IFormFile file, [FromForm] int width = 0, [FromForm] int height = 0, string format = "", CancellationToken cancellationToken = default)
    {
        if (file == null)
            return BadRequest("Field \"file\": cannot be empty");

        if (file.Length <= 0)
            return BadRequest("Field \"file\": stream cannot have zero length");

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

        return Ok(uploadedVideoSrc);
    }

    [HttpPost("Complex")]
    [RequestSizeLimit(100_000_000)]
    public async Task<ActionResult<IEnumerable<FileSrc<VideoMeta>>>> UploadVideo([FromForm] IFormFile file, bool removeAudio = false, CancellationToken cancellationToken = default)
    {
        if (file == null)
            return BadRequest("Field \"file\": cannot be empty");

        if (file.Length <= 0)
            return BadRequest("Field \"file\": stream cannot have zero length");

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

        return Ok(uploadedVideoSrcDict.Values);
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

    private static ImmutableList<VideoSetup> GenerateVideoSetups()
    {
        var formats = ImmutableList.Create(VideoFormat.WebM, VideoFormat.Mp4);
        var widths = ImmutableList.Create(480, 640, 960, 1920);

        var setups = formats
            .SelectMany(
                f =>
                    widths.Select(
                        w => new VideoSetup { Format = f, Size = new System.Drawing.Size(w, 0) }
                    )
            )
            .ToImmutableList();

        return setups;
    }
}
