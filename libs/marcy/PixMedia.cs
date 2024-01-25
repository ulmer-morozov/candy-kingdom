using System.Text.Json.Serialization;

namespace CandyKingdom.Marcy;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$$type")]
[JsonDerivedType(typeof(Video), Video.MediaType)]
[JsonDerivedType(typeof(Image), Image.MediaType)]
public abstract record PixMedia
{
    public abstract IEnumerable<MediaSourceBase> Sources { get; }

    public abstract string Type { get; }
}
