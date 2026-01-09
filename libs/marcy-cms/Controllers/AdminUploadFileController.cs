using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Storage;
using CandyKingdom.Marcy.Utilities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CandyKingdom.MarcyCms.Controllers;

[Authorize]
[Route("Api/Admin/Upload/File")]
public sealed class AdminUploadFileController : ControllerBase
{
    private readonly IFileStorage _fileStorage;

    public AdminUploadFileController(IFileStorage fileStorage)
    {
        _fileStorage = fileStorage;
    }

    [HttpPost]
    [RequestSizeLimit(100_000_000)]
    public async Task<Results<BadRequest<string>, Ok<FileSrc<FileMeta>>>> UploadFile([FromForm] IFormFile file, CancellationToken cancellationToken = default)
    {
        if (file == null)
        {
            return TypedResults.BadRequest("Field \"file\": cannot be empty");
        }

        if (file.Length <= 0)
        {
            return TypedResults.BadRequest("Field \"file\": stream cannot have zero length");
        }

        var fileStream = file.OpenReadStream();

        var extension = Path.GetExtension(file.FileName);
        var hash = await fileStream.CalcMd5AsBase62Async(cancellationToken);

        fileStream.Seek(0, SeekOrigin.Begin);

        var fileName = $"file_{hash}{extension}";

        var storedFile = await _fileStorage.Store
        (
            stream: fileStream,
            name: fileName,
            mimeType: file.ContentType,
            cancellationToken: CancellationToken.None
        );

        var fileSrc = new FileSrc<FileMeta>
        {
            MimeType = file.ContentType,
            Url = storedFile.Url,
            Meta = new FileMeta
            {
                ByteCount = fileStream.Position
            }
        };

        return TypedResults.Ok(fileSrc);
    }
}
