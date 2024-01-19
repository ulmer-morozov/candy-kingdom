using System.Text;
namespace CandyKingdom.MarcyCms;

public sealed class StringWriterWithEncoding(Encoding encoding) : StringWriter
{
    public override Encoding Encoding => encoding;
}
