using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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

                using (XmlReader reader = XmlReader.Create(Path.Combine(XmlMainFolderName, XmlFolder, XmlParentFile), GetXmlReaderSettings()))
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
                using (StreamWriter writer = new StreamWriter(File.Open(Path.Combine(LogFileName, ImageMissingLogName), FileMode.Append)))
                {
                    writer.WriteLine($"[{CurrentBuild}] {message}");
                }
            }
        }

        protected void LogReferenceNameNotFound(string message)
        {
            if (Logger)
            {
                using (StreamWriter writer = new StreamWriter(File.Open(Path.Combine(LogFileName, ReferenceLogName), FileMode.Append)))
                {
                    writer.WriteLine($"[{CurrentBuild}] {message}");
                }
            }
        }

        /// <summary>
        /// Convert the string to an integer. If string is empty or null return null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected int? ConvertToNullableInt(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (int.TryParse(value, out int result))
                    return result;
                else
                    return 0;
            }
            else
            {
                return null;
            }
        }

        protected XmlReaderSettings GetXmlReaderSettings()
        {
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreWhitespace = true
            };

            return xmlReaderSettings;
        }

        protected Color ConvertHexToColor(string hex)
        {
            return Color.FromArgb(int.Parse(hex.TrimStart('#'), NumberStyles.AllowHexSpecifier));
        }
    }
}
