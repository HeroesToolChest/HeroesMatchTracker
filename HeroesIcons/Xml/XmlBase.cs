using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace HeroesIcons.Xml
{
    internal abstract class XmlBase : HeroesIconsBase
    {
        protected string XmlParentFile { get; set; }
        protected string XmlFolder { get; set; }

        protected List<string> XmlChildFiles { get; set; } = new List<string>();

        protected virtual void Parse()
        {
            ParseParentFile();
            ParseChildFiles();
        }

        protected abstract void ParseChildFiles();

        protected void ParseParentFile()
        {
            try
            {
                var fileExtension = Path.GetExtension(XmlParentFile);
                if (fileExtension != ".xml")
                    throw new ParseXmlException($"{XmlParentFile} is not an Xml file");

                if (string.IsNullOrEmpty(XmlFolder))
                    throw new ParseXmlException($"Parameter xmlFolder is required");

                if (string.IsNullOrEmpty(fileExtension))
                    XmlParentFile += ".xml";

                using (XmlTextReader reader = new XmlTextReader($@"Xml/{XmlFolder}/{XmlParentFile}"))
                {
                    reader.ReadStartElement(XmlFolder);

                    while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
                    {
                        if (reader.NodeType == XmlNodeType.Comment || reader.NodeType == XmlNodeType.Text || reader.NodeType == XmlNodeType.Whitespace)
                            continue;

                        XElement el = (XElement)XNode.ReadFrom(reader);
                        XmlChildFiles.Add(el.Name.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ParseXmlException("Error on parsing of xml files", ex);
            }
        }
    }
}
