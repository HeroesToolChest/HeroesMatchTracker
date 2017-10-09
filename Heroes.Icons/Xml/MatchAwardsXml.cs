using Heroes.Icons.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Heroes.Icons.Xml
{
    internal class MatchAwardsXml : XmlBase, IMatchAwards
    {
        private readonly string IconFolderName = "Awards";

        private MatchAwardsXml(string parentFile, string xmlBaseFolder, int currentBuild, bool logger)
            : base(currentBuild, logger)
        {
            XmlParentFile = parentFile;
            XmlBaseFolder = xmlBaseFolder;
            XmlFolder = xmlBaseFolder;
        }

        public Dictionary<string, Tuple<string, string>> MVPScreenAwardByAwardType { get; private set; } = new Dictionary<string, Tuple<string, string>>();
        public Dictionary<string, Tuple<string, string>> MVPScoreScreenAwardByAwardType { get; private set; } = new Dictionary<string, Tuple<string, string>>();
        public Dictionary<string, string> MVPAwardDescriptionByAwardType { get; private set; } = new Dictionary<string, string>();

        public static MatchAwardsXml Initialize(string parentFile, string xmlBaseFolder, int currentBuild, bool logger)
        {
            MatchAwardsXml xml = new MatchAwardsXml(parentFile, xmlBaseFolder, currentBuild, logger);
            xml.Parse();
            return xml;
        }

        /// <summary>
        /// Returns the MVPScreen award Stream of the given mvpAwardType and color
        /// </summary>
        /// <param name="mvpAwardType">Reference name of award</param>
        /// <param name="mvpColor">Color of icon</param>
        /// <param name="awardName"></param>
        /// <returns></returns>
        public Stream GetMVPScreenAward(string mvpAwardType, MVPScreenColor mvpColor, out string awardName)
        {
            if (MVPAwardDescriptionByAwardType.ContainsKey(mvpAwardType))
            {
                var award = MVPScreenAwardByAwardType[mvpAwardType];

                awardName = award.Item1;
                return HeroesIcons.GetHeroesIconsAssembly().GetManifestResourceStream(award.Item2.Replace("{mvpColor}", mvpColor.ToString().ToLowerInvariant()));
            }
            else
            {
                LogReferenceNameNotFound($"MVP screen award type: {mvpAwardType}");
                awardName = mvpAwardType;
                return HeroesIcons.GetHeroesIconsAssembly().GetManifestResourceStream(SetImageStreamString($"{TalentFolderName}.{TalentGenericFolderName}", NoTalentIconFound));
            }
        }

        /// <summary>
        /// Returns the ScoreScreen award Stream of the given mvpAwardType and color
        /// </summary>
        /// <param name="mvpAwardType">Reference name of award</param>
        /// <param name="mvpColor">Color of icon</param>
        /// <param name="awardName"></param>
        /// <returns></returns>
        public Stream GetMVPScoreScreenAward(string mvpAwardType, MVPScoreScreenColor mvpColor, out string awardName)
        {
            if (MVPScoreScreenAwardByAwardType.ContainsKey(mvpAwardType))
            {
                var award = MVPScoreScreenAwardByAwardType[mvpAwardType];

                awardName = award.Item1;
                return HeroesIcons.GetHeroesIconsAssembly().GetManifestResourceStream(award.Item2.Replace("{mvpColor}", mvpColor.ToString().ToLowerInvariant()));
            }
            else
            {
                LogReferenceNameNotFound($"MVP score screen award type: {mvpAwardType}");
                awardName = mvpAwardType;
                return HeroesIcons.GetHeroesIconsAssembly().GetManifestResourceStream(SetImageStreamString($"{TalentFolderName}.{TalentGenericFolderName}", NoTalentIconFound));
            }
        }

        /// <summary>
        /// Returns the decription of the award
        /// </summary>
        /// <param name="mvpAwardType">Reference name of award</param>
        /// <returns></returns>
        public string GetMatchAwardDescription(string mvpAwardType)
        {
            if (string.IsNullOrEmpty(mvpAwardType) || !MVPAwardDescriptionByAwardType.ContainsKey(mvpAwardType))
                return string.Empty;

            return MVPAwardDescriptionByAwardType[mvpAwardType];
        }

        /// <summary>
        /// Returns a list of all the match awards (reference names)
        /// </summary>
        /// <returns></returns>
        public List<string> GetMatchAwardsList()
        {
            return new List<string>(MVPScoreScreenAwardByAwardType.Keys);
        }

        public int TotalCountOfAwards()
        {
            return XmlChildFiles.Count;
        }

        protected override void ParseChildFiles()
        {
            try
            {
                foreach (var award in XmlChildFiles)
                {
                    using (XmlReader reader = XmlReader.Create(Path.Combine(XmlMainFolderName, XmlBaseFolder, $"{award}{DefaultFileExtension}"), GetXmlReaderSettings()))
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
                                                var awardTuple = new Tuple<string, string>(realAwardName, SetImageStreamString(IconFolderName, reader.Value));

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
                throw new ParseXmlException($"Error on xml parsing of {XmlParentFile}", ex);
            }
        }
    }
}
