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
                TypeDiscriminatorPropertyName = "Type",
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new JsonDerivedType(typeof(TextBone)),
                    new JsonDerivedType(typeof(MediaBone)),
                    new JsonDerivedType(typeof(VimeoBone)),
                    new JsonDerivedType(typeof(PageListBone)),
                }
            };
        }

        return jsonTypeInfo;
    }
}
