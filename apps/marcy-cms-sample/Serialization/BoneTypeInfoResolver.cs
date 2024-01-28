using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

using CandyKingdom.Marcy.Pages;
using CandyKingdom.Marcy.Skeleton;
using CandyKingdom.MarcyCms.Sample.Bones;
using CandyKingdom.MarcyCms.Sample.PageTypes;
using CandyKingdom.MarcyCms.Settings;

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
                    new JsonDerivedType(typeof(TextBone),TextBone.BoneType),
                    new JsonDerivedType(typeof(MediaBone), MediaBone.BoneType),
                    new JsonDerivedType(typeof(VimeoBone),VimeoBone.BoneType),
                    new JsonDerivedType(typeof(PageListBone), PageListBone.BoneType),
                }
            };
        }

        if (jsonTypeInfo.Type == typeof(PageData))
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$type",
                IgnoreUnrecognizedTypeDiscriminators = false,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new JsonDerivedType(typeof(ProjectPageData), ProjectPageData.PageDataType)
                }
            };
        }

        if (jsonTypeInfo.Type == typeof(Setting))
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                IgnoreUnrecognizedTypeDiscriminators = false,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new JsonDerivedType(typeof(Setting<SettingData>)),
                }
            };
        }

        if (jsonTypeInfo.Type == typeof(SettingData))
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$type",
                IgnoreUnrecognizedTypeDiscriminators = false,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new JsonDerivedType(typeof(TextSettingData), TextSettingData.SettingDataType),
                    new JsonDerivedType(typeof(LocalizedTextSettingData), LocalizedTextSettingData.SettingDataType)
                }
            };
        }

        return jsonTypeInfo;
    }
}
