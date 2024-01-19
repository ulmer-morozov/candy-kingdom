using System.Collections.Immutable;
using System.Drawing;
using CandyKingdom.Marcy;
using CandyKingdom.Marcy.ImageTools;
using CandyKingdom.MarcyCms.Sample.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CandyKingdom.MarcyCms.Sample.Controllers.Admin;

[Authorize(Policy = Roles.Admin)]
[Route("Api/Admin/Upload/Image")]
public sealed class AdminUploadImageController : Controller
{
    public static readonly ImmutableList<ImageSetup> DefaultSetups = GenerateImageSetups();

    private readonly IImageUploader _imageUploader;

    public AdminUploadImageController(IImageUploader imageUploader)
    {
        _imageUploader = imageUploader;
    }

    [HttpPost("Raw")]
    [RequestSizeLimit(50_000_000)]
    public async Task<ActionResult<FileSrc<ImageMeta>>> UploadRawImage([FromForm] IFormFile file, CancellationToken cancellationToken = default)
    {
        if (file == null)
            return BadRequest("Field \"file\": cannot be empty");

        if (file.Length <= 0)
            return BadRequest("Field \"file\": stream cannot have zero length");

        using var imageMemoryStream = new MemoryStream();

        await file.OpenReadStream().CopyToAsync(imageMemoryStream, cancellationToken);

        var imageSetup = new ImageSetup
        {
            Type = ImageTransformType.Original,
        };

        var convertParameters = new ImageConvertParameters
        {
            Minify = false
        };

        var uploadedImageSrc = await _imageUploader.ConvertAndStore
        (
            imageMemoryStream,
            imageSetup,
            convertParameters,
            cancellationToken
        );

        return Ok(uploadedImageSrc);
    }

    [HttpPost]
    [RequestSizeLimit(50_000_000)]
    public async Task<ActionResult<FileSrc<ImageMeta>>> UploadSingleImage([FromForm] IFormFile file, [FromForm] int width = 0, [FromForm] int height = 0, string format = "", CancellationToken cancellationToken = default)
    {
        if (file == null)
            return BadRequest("Field \"file\": cannot be empty");

        if (file.Length <= 0)
            return BadRequest("Field \"file\": stream cannot have zero length");

        using var imageMemoryStream = new MemoryStream();

        await file.OpenReadStream().CopyToAsync(imageMemoryStream, cancellationToken);

        var imageSetup = new ImageSetup
        {
            Type = ImageTransformType.CropResize,
            Size = new Size(width, height),
            Format = GetFormatByExtension(string.IsNullOrWhiteSpace(format) ? file.Name : format)
        };

        var convertParameters = new ImageConvertParameters
        {
            Minify = true
        };

        var uploadedImageSrc = await _imageUploader.ConvertAndStore
        (
            imageMemoryStream,
            imageSetup,
            convertParameters,
            cancellationToken
        );

        return Ok(uploadedImageSrc);
    }

    [HttpPost("Complex")]
    [RequestSizeLimit(50_000_000)]
    public async Task<ActionResult<IEnumerable<FileSrc<ImageMeta>>>> UploadImage([FromForm] IFormFile file, CancellationToken cancellationToken = default)
    {
        if (file == null)
            return BadRequest("Field \"file\": cannot be empty");

        if (file.Length <= 0)
            return BadRequest("Field \"file\": stream cannot have zero length");

        using var imageMemoryStream = new MemoryStream();

        await file.OpenReadStream().CopyToAsync(imageMemoryStream, cancellationToken);

        var convertParameters = new ImageConvertParameters
        {
            Minify = true
        };

        var uploadedImageDict = await _imageUploader.ConvertAndStore
        (
            imageStream: imageMemoryStream,
            setups: DefaultSetups,
            convertParameters,
            cancellationToken
        );

        return Ok(uploadedImageDict.Values);
    }

    private static ImageFormat GetFormatByExtension(string fileName)
    {
        var extenision = Path.GetExtension(fileName).ToLowerInvariant();

        return extenision switch
        {
            ".png" => ImageFormat.Png,
            ".jpg" or ".jpeg" => ImageFormat.Jpeg,
            ".webp" => ImageFormat.WebP,
            _ => ImageFormat.Empty,
        };
    }

    private static ImmutableList<ImageSetup> GenerateImageSetups()
    {
        // todo: add formats based on image transparency
        // for example Webp|Png
        ImmutableList<ImageFormat> imageFormats = [ImageFormat.WebP, ImageFormat.Jpeg];
        ImmutableList<int> widths = [160, 320, 480, 640, 960, 1280, 1440, 1920, 2560];

        var setups = imageFormats
            .SelectMany(
                f =>
                    widths.Select(
                        w => new ImageSetup { Format = f, Size = new System.Drawing.Size(w, 0) }
                    )
            )
            .ToImmutableList();

        return setups;
    }
}
