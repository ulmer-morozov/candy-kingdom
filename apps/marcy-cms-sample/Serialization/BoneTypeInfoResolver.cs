using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

using CandyKingdom.Marcy.Skeleton;
using CandyKingdom.MarcyCms.Sample.Bones;

namespace CandyKingdom.MarcyCms.Sample.Serialization;

public sealed class BoneTypeInfoResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        var jsonTypeInfo = base.GetTypeInfo(type, options);

        if (jsonTypeInfo.Type == typeof(Bone))
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$type",
                IgnoreUnrecognizedTypeDiscriminators = false,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new JsonDerivedType(typeof(TextBone),TextBone.TYPE),
                    new JsonDerivedType(typeof(MediaBone), MediaBone.TYPE),
                    new JsonDerivedType(typeof(VimeoBone),VimeoBone.TYPE),
                    new JsonDerivedType(typeof(PageListBone), PageListBone.TYPE),
                }
            };
        }

        return jsonTypeInfo;
    }
}
