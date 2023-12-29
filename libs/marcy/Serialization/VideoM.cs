namespace CandyKingdom.Marcy.Serialization;

public sealed record VideoM : MediaM
{
    public VideoM(IEnumerable<MediaSourceM> sources)
        : base(sources) { }

    public VideoM(params MediaSourceM[] sources)
        : base(sources) { }

    public VideoM(string path)
        : this(new[] { new MediaSourceM { Path = path } }) { }
}
