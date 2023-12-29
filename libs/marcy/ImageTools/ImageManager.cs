using CandyKingdom.Marcy.Utilities;
using NetVips;

namespace CandyKingdom.Marcy.ImageTools;

public sealed class ImageManager : IImageManager
{
  public bool HasTransparency(Stream imageStream)
  {
    if (imageStream.CanSeek)
      imageStream.Seek(0, SeekOrigin.Begin);

    using var image = NetVips.Image.NewFromStream(imageStream, access: Enums.Access.Random);

    var hasAlphaChannel = image.BandExists(3);

    if (!hasAlphaChannel)
      return false;

    var bands = image.Bandsplit();
    var alphaBand = bands[3];

    const int targetSize = 100;
    var scale = (float)targetSize / Math.Max(image.Width, image.Height);

    using var resizedAlphaBand = alphaBand.Resize(scale);

    for (var j = 0; j < resizedAlphaBand.Height; j++)
    for (var i = 0; i < resizedAlphaBand.Width; i++)
    {
      var pixel = resizedAlphaBand.Getpoint(i, j);

      var a = pixel[0];

      if (a < 255)
        return true;
    }

    return false;
  }

  public async Task Convert(
    Stream sourceStream,
    ICollection<ImageSetup> setups,
    Func<ImageSetup, InMemoryImage, Task> action,
    CancellationToken cancellationToken = default
  )
  {
    using var sourceImage = NetVips.Image.NewFromStream(sourceStream, access: Enums.Access.Random);

    foreach (var setup in setups)
    {
      if (sourceImage.Width < setup.Size.Width || sourceImage.Height < setup.Size.Height)
      {
        // don't create image for that case
        continue;
      }

      Console.WriteLine($"Converting for image setup: {setup}");

      NetVips.Image? croppedImage = null;

      var (left, top, cropWidth, cropHeight, finalWidth, _, needCrop) = MathUtils.CenterCrop(
        sourceImage.Width,
        sourceImage.Height,
        setup.Size.Width,
        setup.Size.Height
      );

      if (needCrop)
      {
        croppedImage = sourceImage.Crop(top, left, cropWidth, cropHeight);
      }

      var scale = (float)finalWidth / sourceImage.Width;

      var inMemoryImage = CreateResizedImage(
        croppedImage ?? sourceImage,
        setup.Format,
        scale: scale
      );

      await action(setup, inMemoryImage);

      croppedImage?.Dispose();
    }
  }

  private static InMemoryImage CreateResizedImage(
    NetVips.Image sourceImage,
    ImageFormat format,
    double scale
  )
  {
    using var targetImage = sourceImage.Resize(scale);
    var resizedStream = new MemoryStream();

    // for better quality webp
    if (format == ImageFormat.Webp)
    {
      targetImage.WebpsaveStream(
        resizedStream,
        nearLossless: true,
        smartSubsample: true,
        strip: true
      );
    }
    else
    {
      targetImage.WriteToStream(resizedStream, format.Extension);
    }

    resizedStream.Seek(0, SeekOrigin.Begin);

    var inMemoryImage = new InMemoryImage
    {
      Stream = resizedStream,
      Format = format,
      Meta = new ImageMeta
      {
        Width = targetImage.Width,
        Height = targetImage.Height,
        ByteCount = resizedStream.Length
      }
    };

    return inMemoryImage;
  }
}
