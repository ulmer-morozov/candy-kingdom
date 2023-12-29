using TypeGen.Core.Converters;
using System.Reflection;
using System.Text.Json.Serialization;

namespace CandyKingdom.MarcyGen;

public class JsonMemberNameConverter : IMemberNameConverter
{
    public string Convert(string name, MemberInfo memberInfo)
    {
        var attribute = memberInfo.GetCustomAttribute<JsonPropertyNameAttribute>();
        return attribute != null ? attribute.Name : name;
    }
}
