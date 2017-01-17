using System;
using System.Collections.Generic;
using System.Xml;

namespace Heroes.Icons.Xml
{
    internal class MatchAwardsXml : XmlBase
    {
        private MatchAwardsXml(string parentFile, string xmlFolder)
        {
            XmlParentFile = parentFile;
            XmlFolder = xmlFolder;
        }

        public Dictionary<string, Tuple<string, Uri>> MVPScreenAwards { get; private set; } = new Dictionary<string, Tuple<string, Uri>>();
        public Dictionary<string, Tuple<string, Uri>> MVPScoreScreenAwards { get; private set; } = new Dictionary<string, Tuple<string, Uri>>();
        public Dictionary<string, string> MVPAwardDescriptions { get; private set; } = new Dictionary<string, string>();

        public static MatchAwardsXml Initialize(string parentFile, string xmlFolder)
        {
            MatchAwardsXml xml = new MatchAwardsXml(parentFile, xmlFolder);
            xml.Parse();
            return xml;
        }

        protected override void ParseChildFiles()
        {
            try
            {
                foreach (var award in XmlChildFiles)
                {
                    using (XmlReader reader = XmlReader.Create($@"Xml\{XmlFolder}\{award}.xml"))
                    {
                        reader.MoveToContent();

                        if (reader.Name != award)
                            continue;

                        // get the real award name
                        // example MasteroftheCurse -> (real) Master of the Curse
                        string realAwardName = reader["name"];
                        if (string.IsNullOrEmpty(realAwardName))
                            realAwardName = award; // default to award name

                        while (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                string awardType = reader.Name; // example: MostDamageTaken

                                while (reader.Read() && reader.Name != awardType)
                                {
                                    if (reader.NodeType == XmlNodeType.Element)
                                    {
                                        string elementName = reader.Name;

                                        if (reader.Read())
                                        {
                                            if (elementName == "Description")
                                            {
                                                MVPAwardDescriptions.Add(awardType, reader.Value);
                                            }
                                            else
                                            {
                                                var awardTuple = new Tuple<string, Uri>(realAwardName, SetMVPAwardUri(reader.Value));

                                                if (elementName == "MVPScreen")
                                                    MVPScreenAwards.Add(awardType, awardTuple);
                                                else if (elementName == "ScoreScreen")
                                                    MVPScoreScreenAwards.Add(awardType, awardTuple);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ParseXmlException("Error on parsing of xml files", ex);
            }
        }

        private Uri SetMVPAwardUri(string fileName)
        {
            return new Uri($@"{ApplicationIconsPath}\Awards\{fileName}", UriKind.Absolute);
        }
    }
}
