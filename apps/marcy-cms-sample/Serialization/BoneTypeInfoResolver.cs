using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using CandyKingdom.Marcy.Skeleton;

namespace CandyKingdom.MarcyCms.Sample.Bones;

public sealed class BoneTypeInfoResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

        Type baseBoneType = typeof(Bone);

        if (jsonTypeInfo.Type == baseBoneType)
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = nameof(TextBone.Type),
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
