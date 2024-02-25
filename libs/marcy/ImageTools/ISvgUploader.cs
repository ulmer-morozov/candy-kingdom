namespace CandyKingdom.Marcy.ImageTools;

public enum SvgConvertError
{
    BadXmlMarkup = 1,
    NotAnSvgFile = 2
}

public sealed record SvgConvertParameters
{
    public bool AddViewBox { get; init; }
}

public interface ISvgUploader
{
    public Task<ResultOrError<FileSrc<SvgMeta>>> ConvertAndStore(
      Stream svgStream,
      SvgConvertParameters convertParameters,
      CancellationToken cancellationToken
    );
}
