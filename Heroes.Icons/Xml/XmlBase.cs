using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Heroes.Icons.Xml
{
    internal abstract class XmlBase : HeroesBase
    {
        private bool Logger;

        protected XmlBase(int currentBuild, bool logger)
        {
            CurrentBuild = currentBuild;
            Logger = logger;
        }

        protected int CurrentBuild { get; }

        protected string XmlParentFile { get; set; }

        /// <summary>
        /// The base folder. Should only contain a single folder, use XmlFolder to include a sub folder
        /// </summary>
        protected string XmlBaseFolder { get; set; }

        /// <summary>
        /// May contain a sub folder
        /// </summary>
        protected string XmlFolder { get; set; }

        protected List<string> XmlChildFiles { get; set; } = new List<string>();

        protected virtual void Parse()
        {
            ParseParentFile();
            ParseChildFiles();
        }

        protected abstract void ParseChildFiles();

        protected virtual void ParseParentFile()
        {
            try
            {
                if (!ValidateRequiredFiles())
                    return;

                using (XmlTextReader reader = new XmlTextReader($@"Xml\{XmlFolder}\{XmlParentFile}"))
                {
                    reader.ReadStartElement();

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

        protected bool ValidateRequiredFiles()
        {
            bool valid = true;

            var fileExtension = Path.GetExtension(XmlParentFile);
            if (fileExtension != ".xml")
            {
                valid = false;
                throw new ParseXmlException($"{XmlParentFile} is not an Xml file");
            }

            if (string.IsNullOrEmpty(XmlBaseFolder))
            {
                valid = false;
                throw new ParseXmlException($"Parameter xmlBaseFolder is required");
            }

            if (string.IsNullOrEmpty(XmlFolder))
            {
                valid = false;
                throw new ParseXmlException($"Parameter xmlFolder is required");
            }

            return valid;
        }

        protected void LogMissingImage(string message)
        {
            if (Logger)
            {
                using (StreamWriter writer = new StreamWriter($"{LogFileName}/{ImageMissingLogName}", true))
                {
                    writer.WriteLine($"[{CurrentBuild}] {message}");
                }
            }
        }

        protected void LogReferenceNameNotFound(string message)
        {
            if (Logger)
            {
                using (StreamWriter writer = new StreamWriter($"{LogFileName}/{ReferenceLogName}", true))
                {
                    writer.WriteLine($"[{CurrentBuild}] {message}");
                }
            }
        }
    }
}
