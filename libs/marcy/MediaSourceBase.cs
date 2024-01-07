namespace CandyKingdom.Marcy;

public abstract record MediaSourceBase
{
    public virtual IEnumerable<FileSrcBase> SrcSet { get; }

    protected MediaSourceBase(IEnumerable<FileSrcBase> srcSet)
    {
        SrcSet = srcSet;
    }
}
