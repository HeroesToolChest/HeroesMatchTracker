using System;
using System.Collections.Generic;
using System.Xml;

namespace Heroes.Icons.Xml
{
    internal class MatchAwardsXml : XmlBase
    {
        private MatchAwardsXml(string parentFile, string xmlBaseFolder)
        {
            XmlParentFile = parentFile;
            XmlBaseFolder = xmlBaseFolder;
            XmlFolder = xmlBaseFolder;
        }

        public Dictionary<string, Tuple<string, Uri>> MVPScreenAwardByAwardType { get; private set; } = new Dictionary<string, Tuple<string, Uri>>();
        public Dictionary<string, Tuple<string, Uri>> MVPScoreScreenAwardByAwardType { get; private set; } = new Dictionary<string, Tuple<string, Uri>>();
        public Dictionary<string, string> MVPAwardDescriptionByAwardType { get; private set; } = new Dictionary<string, string>();

        public static MatchAwardsXml Initialize(string parentFile, string xmlBaseFolder)
        {
            MatchAwardsXml xml = new MatchAwardsXml(parentFile, xmlBaseFolder);
            xml.Parse();
            return xml;
        }

        protected override void ParseChildFiles()
        {
            try
            {
                foreach (var award in XmlChildFiles)
                {
                    using (XmlReader reader = XmlReader.Create($@"Xml\{XmlBaseFolder}\{award}.xml"))
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
                                                MVPAwardDescriptionByAwardType.Add(awardType, reader.Value);
                                            }
                                            else
                                            {
                                                var awardTuple = new Tuple<string, Uri>(realAwardName, SetMVPAwardUri(reader.Value));

                                                if (elementName == "MVPScreen")
                                                    MVPScreenAwardByAwardType.Add(awardType, awardTuple);
                                                else if (elementName == "ScoreScreen")
                                                    MVPScoreScreenAwardByAwardType.Add(awardType, awardTuple);
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
