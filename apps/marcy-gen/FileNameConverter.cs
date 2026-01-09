using TypeGen.Core.Converters;

namespace CandyKingdom.MarcyGen;

public class FileNameConverter : ITypeNameConverter
{
    public string Convert(string name, Type type)
    {
        if (TypeNameConverter.SpecialTypes.Contains(type))
        {
            return $"{name}-base";
        }

        return name;
    }
}
