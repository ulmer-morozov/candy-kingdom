namespace CandyKingdom.Marcy;

public abstract record PixMedia
{
    public virtual IEnumerable<MediaSourceBase> Sources { get; }

    public abstract string Type { get; }

    protected PixMedia(IEnumerable<MediaSourceBase> sources)
    {
        Sources = sources;
    }
}
