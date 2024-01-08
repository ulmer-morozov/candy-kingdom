using System.Text;
namespace CandyKingdom.MarcyCms;

public sealed class StringWriterWithEncoding(Encoding encoding) : StringWriter
{
    private readonly Encoding encoding = encoding;

    public override Encoding Encoding
    {
        get { return encoding; }
    }
}
