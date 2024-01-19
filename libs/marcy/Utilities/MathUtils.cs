namespace CandyKingdom.Marcy.Utilities;

public static class MathUtils
{
    public static (
      int left,
      int top,
      int cropWidth,
      int cropHeight,
      int finalWidth,
      int finalHeight,
      bool needCrop
    ) CenterCrop(int width, int height, int targetWidth = 0, int targetHeight = 0)
    {
        var oldRatio = height == 0 ? 0 : (float)width / height;

        var targetRatio =
          targetWidth == 0 || targetHeight == 0 ? oldRatio : (float)targetWidth / targetHeight;

        int cropWidth;
        int cropHeight;
        bool needCrop;

        if (oldRatio == 0 || targetRatio == 0 || oldRatio == targetRatio)
        {
            cropWidth = width;
            cropHeight = height;
            needCrop = false;
        }
        else if (targetRatio > oldRatio)
        {
            cropWidth = width;
            cropHeight = (int)Math.Round(width / targetRatio);
            needCrop = true;
        }
        else // if targetRatio < oldRatio
        {
            cropWidth = (int)Math.Round(height * targetRatio);
            cropHeight = height;
            needCrop = true;
        }

        var scale =
          targetWidth != 0
            ? (float)targetWidth / cropWidth
            : targetHeight != 0
              ? (float)targetHeight / cropHeight
              : 1;

        var finalWidth = (int)Math.Round(scale * cropWidth);
        var finalHeight = (int)Math.Round(scale * cropHeight);

        return (
          left: (int)Math.Ceiling((width - cropWidth) / 2d),
          top: (int)Math.Ceiling((height - cropHeight) / 2d),
          cropWidth,
          cropHeight,
          finalWidth,
          finalHeight,
          needCrop
        );
    }
}
