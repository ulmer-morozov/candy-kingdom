using System.Text.RegularExpressions;

namespace CandyKingdom.Marcy.Utilities;

public static partial class ParseUtils
{
    public static bool IsValidHtmlColor(string inputColor)
    {
        //regex from http://stackoverflow.com/a/1636354/2343
        return ColorHexRegex().Match(inputColor).Success;
    }

    [GeneratedRegex("^#(?:[0-9a-fA-F]{3}){1,2}$")]
    private static partial Regex ColorHexRegex();
}
