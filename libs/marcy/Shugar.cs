namespace CandyKingdom.Marcy;

public static class Shugar
{
    public static Video Video(params VideoSource[] sources)
    {
        return new Video(sources);
    }

    public static Image Image(params ImageSource[] sources)
    {
        return new Image(sources);
    }

    public static VideoSource VideoSource(params FileSrc<VideoMeta>[] srcSet)
    {
        return new VideoSource(srcSet);
    }

    public static ImageSource ImageSource(params FileSrc<ImageMeta>[] srcSet)
    {
        return new ImageSource(srcSet);
    }
}
