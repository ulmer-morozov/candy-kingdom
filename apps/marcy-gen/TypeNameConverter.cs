using System.Collections.Immutable;

using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Settings;

using TypeGen.Core.Converters;

namespace CandyKingdom.MarcyGen;

public class TypeNameConverter : ITypeNameConverter
{
    // These needed for successfull generation of generic classes that have same name as base class
    public static readonly ImmutableList<Type> SpecialTypes = [typeof(Page), typeof(Setting)];

    public string Convert(string name, Type type)
    {
        if (SpecialTypes.Contains(type))
        {
            return $"{name}Base";
        }

        return name;
    }
}
