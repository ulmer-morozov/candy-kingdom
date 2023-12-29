using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace CandyKingdom.Marcy.Serialization;

[JsonDerivedType(typeof(ImageM))]
[JsonDerivedType(typeof(VideoM))]
public record MediaM
{
  public ImmutableList<MediaSourceM> Sources { get; init; } = ImmutableList<MediaSourceM>.Empty;

  public MediaM() { }

  public MediaM(IEnumerable<MediaSourceM> sources)
  {
    Sources = sources as ImmutableList<MediaSourceM> ?? sources.ToImmutableList();
  }

  public MediaM(params MediaSourceM[] sources)
    : this(sources as IEnumerable<MediaSourceM>) { }
}
