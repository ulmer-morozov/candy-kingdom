using System.Text;
using System.Xml;

namespace CandyKingdom.MarcyCms;

public sealed class SitemapBuilder
{
    public static string BuildXml(IEnumerable<SitemapItem> items, Encoding encoding)
    {
        var xmlBuilder = new StringWriterWithEncoding(encoding);

        using (var xml = XmlWriter.Create(xmlBuilder, new XmlWriterSettings
        {
            Indent = true,
            Encoding = encoding
        }))
        {
            xml.WriteStartDocument();
            xml.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
            xml.WriteAttributeString("xmlns", "xhtml", null, "http://www.w3.org/1999/xhtml");

            foreach (var item in items)
            {
                xml.WriteStartElement("url");
                xml.WriteElementString("loc", item.Loc);

                foreach (var alternate in item.Alternates)
                {
                    xml.WriteStartElement("xhtml", "link", null);
                    xml.WriteAttributeString("rel", "alternate");
                    xml.WriteAttributeString("hreflang", alternate.HrefLang);
                    xml.WriteAttributeString("href", alternate.Href);
                    xml.WriteEndElement();
                }

                xml.WriteElementString("changefreq", item.ChangeFreq);
                xml.WriteElementString("priority", item.Priority);
                xml.WriteEndElement();
            }

            xml.WriteEndElement();
        }

        var xmlText = xmlBuilder.ToString();

        return xmlText;
    }
}
