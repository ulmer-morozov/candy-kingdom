using System.Collections.Immutable;
using System.Text.Json;
using CandyKingdom.Marcy.ImageTools;

namespace CandyKingdom.Marcy.Serialization;

public interface ISmartJsonProducer
{
    public string SerializeAndInject<T>(
      T obj,
      JsonSerializerOptions jsonSerializerOptions,
      ImageConvertParameters convertParameters,
      ImmutableList<ImageSetup> imageSetups,
      ImmutableList<VideoSetup> videoSetups
    );
}
