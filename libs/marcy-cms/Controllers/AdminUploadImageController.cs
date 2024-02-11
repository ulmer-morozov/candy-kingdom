using System.Collections.Immutable;
using System.Drawing;

using CandyKingdom.Marcy;
using CandyKingdom.Marcy.ImageTools;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CandyKingdom.MarcyCms.Controllers;

[Authorize]
[Route("Api/Admin/Upload/Image")]
public sealed class AdminUploadImageController : ControllerBase
{
    public static readonly ImmutableList<ImageSetup> DefaultSetups = GenerateImageSetups();

    private readonly IImageUploader _imageUploader;
    private readonly ISvgUploader _svgUploader;
    private readonly ILogger<AdminUploadImageController> _logger;

    public AdminUploadImageController(IImageUploader imageUploader, ISvgUploader svgUploader, ILogger<AdminUploadImageController> logger)
    {
        _imageUploader = imageUploader;
        _svgUploader = svgUploader;
        _logger = logger;
    }

    [HttpPost("SVG")]
    public async Task<Results<BadRequest<string>, Ok<FileSrc<SvgMeta>>>> UploadSvgImage([FromForm] IFormFile file, CancellationToken cancellationToken = default)
    {
        if (file == null)
        {
            return TypedResults.BadRequest("Field \"file\": cannot be empty");
        }

        if (file.Length <= 0)
        {
            return TypedResults.BadRequest("Field \"file\": stream cannot have zero length");
        }

        const string allowedMime = "image/svg+xml";

        if (file.ContentType != allowedMime)
        {
            return TypedResults.BadRequest($"Field \"file\": should have content type {allowedMime}, but it was {file.ContentType}");
        }

        var fileStream = file.OpenReadStream();

        var convertParameters = new SvgConvertParameters
        {
            AddViewBox = true
        };

        var fileSrcResult = await _svgUploader.ConvertAndStore(fileStream, convertParameters, cancellationToken);

        if (!fileSrcResult.IsSuccessful && fileSrcResult.ErrorCode == (int)SvgConvertError.BadXmlMarkup)
        {
            return TypedResults.BadRequest(fileSrcResult.Message);
        }

        if (!fileSrcResult.IsSuccessful && fileSrcResult.ErrorCode == (int)SvgConvertError.NotAnSvgFile)
        {
            return TypedResults.BadRequest(fileSrcResult.Message);
        }

        if (!fileSrcResult.IsSuccessful && fileSrcResult.ErrorCode == (int)SvgConvertError.NotAnSvgFile)
        {
            return TypedResults.BadRequest(fileSrcResult.Message);
        }

        if (!fileSrcResult.IsSuccessful)
        {
            _logger.LogError(fileSrcResult);

            throw new Exception(fileSrcResult.Message);
        }

        return TypedResults.Ok(fileSrcResult.Data);
    }


    [HttpPost("Raw")]
    [RequestSizeLimit(50_000_000)]
    public async Task<Results<BadRequest<string>, Ok<FileSrc<ImageMeta>>>> UploadRawImage([FromForm] IFormFile file, CancellationToken cancellationToken = default)
    {
        if (file == null)
        {
            return TypedResults.BadRequest("Field \"file\": cannot be empty");
        }

        if (file.Length <= 0)
        {
            return TypedResults.BadRequest("Field \"file\": stream cannot have zero length");
        }

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

        return TypedResults.Ok(uploadedImageSrc);
    }

    [HttpPost]
    [RequestSizeLimit(50_000_000)]
    public async Task<Results<BadRequest<string>, Ok<FileSrc<ImageMeta>>>> UploadSingleImage([FromForm] IFormFile file, [FromForm] int width = 0, [FromForm] int height = 0, string format = "", CancellationToken cancellationToken = default)
    {
        if (file == null)
        {
            return TypedResults.BadRequest("Field \"file\": cannot be empty");
        }

        if (file.Length <= 0)
        {
            return TypedResults.BadRequest("Field \"file\": stream cannot have zero length");
        }

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

        return TypedResults.Ok(uploadedImageSrc);
    }

    [HttpPost("Complex")]
    [RequestSizeLimit(50_000_000)]
    public async Task<Results<BadRequest<string>, Ok<Image>>> UploadImage([FromForm] IFormFile file, CancellationToken cancellationToken = default)
    {
        if (file == null)
        {
            return TypedResults.BadRequest("Field \"file\": cannot be empty");
        }

        if (file.Length <= 0)
        {
            return TypedResults.BadRequest("Field \"file\": stream cannot have zero length");
        }

        using var imageMemoryStream = new MemoryStream();

        await file.OpenReadStream().CopyToAsync(imageMemoryStream, cancellationToken);

        imageMemoryStream.Seek(0, SeekOrigin.Begin);

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

        var image = new Image(new[]{
            new ImageSource(uploadedImageDict.Values)
        });

        return TypedResults.Ok(image);
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
                        w => new ImageSetup { Format = f, Size = new Size(w, 0) }
                    )
            )
            .ToImmutableList();

        return setups;
    }
}
