using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using CandyKingdom.MarcyCms.Sample.Bones;
using CandyKingdom.MarcyCms.Settings;

namespace CandyKingdom.MarcyCms.Sample.Serialization;

public sealed class SettingDataTypeInfoResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

        Type baseBoneType = typeof(SettingData);

        if (jsonTypeInfo.Type == baseBoneType)
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = nameof(SettingData.Type),
                IgnoreUnrecognizedTypeDiscriminators = false,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new JsonDerivedType(typeof(TextSettingData),TextSettingData.TYPE),
                    new JsonDerivedType(typeof(LocalizedTextSettingData),LocalizedTextSettingData.TYPE),
                    new JsonDerivedType(typeof(VimeoBone)),
                    new JsonDerivedType(typeof(PageListBone)),
                }
            };
        }

        return jsonTypeInfo;
    }
}
