using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Sample.PageTypes;

namespace CandyKingdom.MarcyCms.Sample.Serialization;

public sealed class PageDataInfoResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        var jsonTypeInfo = base.GetTypeInfo(type, options);

        if (jsonTypeInfo.Type == typeof(PageData))
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = nameof(PageData.Type),
                IgnoreUnrecognizedTypeDiscriminators = false,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new JsonDerivedType(typeof(ProjectPageData), ProjectPageData.TYPE)
                }
            };
        }

        return jsonTypeInfo;
    }
}
