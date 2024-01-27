using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

using CandyKingdom.MarcyCms.Settings;

namespace CandyKingdom.MarcyCms.Sample.Serialization;

public sealed class SettingDataTypeInfoResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        var jsonTypeInfo = base.GetTypeInfo(type, options);

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
