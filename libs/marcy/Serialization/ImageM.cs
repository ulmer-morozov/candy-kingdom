namespace CandyKingdom.Marcy.Serialization;

public sealed record ImageM : MediaM
{
    public ImageM(IEnumerable<MediaSourceM> sources)
      : base(sources) { }

    public ImageM(params MediaSourceM[] sources)
      : base(sources) { }

    public ImageM(string path)
      : this(new[] { new MediaSourceM { Path = path } }) { }
}
