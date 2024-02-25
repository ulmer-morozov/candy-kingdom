using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;

using CandyKingdom.Marcy.Storage;
using CandyKingdom.Marcy.Utilities;

namespace CandyKingdom.Marcy.ImageTools;

public sealed partial class SvgUploader : ISvgUploader
{
    private readonly IFileStorage _fileStorage;

    public SvgUploader(IFileStorage fileStorage)
    {
        _fileStorage = fileStorage;
    }

    public async Task<ResultOrError<FileSrc<SvgMeta>>> ConvertAndStore(Stream svgStream, SvgConvertParameters convertParameters, CancellationToken cancellationToken)
    {
        string svgContent;

        using (var textReader = new StreamReader(svgStream))
        {
            svgContent = await textReader.ReadToEndAsync(cancellationToken);
        }

        var svgDocument = new XmlDocument();

        try
        {
            svgDocument.LoadXml(svgContent);
        }

        catch (XmlException e)
        {
            return ResultOrError.Fail<FileSrc<SvgMeta>>(e.Message, (int)SvgConvertError.BadXmlMarkup);
        }

        if (svgDocument.DocumentElement == null)
        {
            return ResultOrError.Fail<FileSrc<SvgMeta>>("SVG image cannot be empty XML document", (int)SvgConvertError.BadXmlMarkup);
        }

        if (svgDocument.DocumentElement.Name?.ToLowerInvariant() != "svg")
        {
            return ResultOrError.Fail<FileSrc<SvgMeta>>($"SVG image should have <svg> tag. But it has {svgDocument.DocumentElement.Name}", (int)SvgConvertError.NotAnSvgFile);
        }

        decimal width;
        decimal height;

        var widthAttribute = svgDocument.DocumentElement.Attributes?["width"];
        var heightAttribute = svgDocument.DocumentElement.Attributes?["height"];
        var viewBoxAttribute = svgDocument.DocumentElement.Attributes?["viewBox"];

        var widthText = CleanDecimal().Replace(widthAttribute?.Value ?? "", "");
        var heightText = CleanDecimal().Replace(heightAttribute?.Value ?? "", "");
        var viewBoxText = viewBoxAttribute?.Value ?? "";

        width = string.IsNullOrWhiteSpace(widthText) ? 0 : decimal.Parse(widthText, CultureInfo.InvariantCulture);
        height = string.IsNullOrWhiteSpace(heightText) ? 0 : decimal.Parse(heightText, CultureInfo.InvariantCulture);

        if (width == 0 && height == 0 && !string.IsNullOrWhiteSpace(viewBoxText))
        {
            var boxParts = viewBoxText.Split(' ');

            if (boxParts.Length == 4)
            {
                var boxWidthText = boxParts[2];
                var boxHeightText = boxParts[3];

                width = string.IsNullOrWhiteSpace(boxWidthText) ? 0 : decimal.Parse(boxWidthText, CultureInfo.InvariantCulture);
                height = string.IsNullOrWhiteSpace(boxHeightText) ? 0 : decimal.Parse(boxHeightText, CultureInfo.InvariantCulture);
            }
        }

        if (convertParameters.AddViewBox && widthAttribute != null && heightText != null && viewBoxAttribute == null)
        {
            svgDocument.DocumentElement.SetAttribute("viewBox", $"0 0 {width} {height}");
        }

        using var newSvgStream = new MemoryStream();

        // using (var stringWriter = new StreamWriter(newSvgStream, Encoding.UTF8))
        using (var xmlTextWriter = XmlWriter.Create(newSvgStream))
        {
            svgDocument.WriteTo(xmlTextWriter);
        }

        newSvgStream.Seek(0, SeekOrigin.Begin);

        var intWidth = (int)Math.Round(width);
        var intHeight = (int)Math.Round(height);

        var hash = await newSvgStream.CalcMd5AsBase62Async(cancellationToken: cancellationToken);

        var fileName = $"graphic_{intWidth}x{intHeight}_{hash}.svg";

        const string mimeType = "image/svg+xml";
        var fileByteCount = newSvgStream.Length;

        var stoiredFile = await _fileStorage.Store
            (
                stream: newSvgStream,
                name: fileName,
                mimeType: mimeType,
                CancellationToken.None
            );

        var fileSrc = new FileSrc<SvgMeta>
        {
            MimeType = mimeType,
            Url = stoiredFile.Url,
            Meta = new SvgMeta
            {
                ByteCount = fileByteCount,
                Width = intWidth,
                Height = intHeight,
                PreciseWidth = width,
                PreciseHeight = height
            }
        };

        return ResultOrError.Success(fileSrc);
    }

    [GeneratedRegex("[^0-9.,]")]
    private static partial Regex CleanDecimal();
}
