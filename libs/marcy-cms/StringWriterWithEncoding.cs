using System.Text;
namespace CandyKingdom.MarcyCms;

public sealed class StringWriterWithEncoding(Encoding encoding) : StringWriter
{
    private readonly Encoding _encoding = encoding;

    public override Encoding Encoding => _encoding;
}
