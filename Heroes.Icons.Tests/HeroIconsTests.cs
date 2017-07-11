using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Heroes.Icons.Tests
{
    [TestClass]
    public class HeroIconsTests
    {
        public HeroIconsTests()
        {
            FrameworkElement frameworkElement = new FrameworkElement(); // needed to load up the framework
        }

        [TestMethod]
        public void HeroesTest()
        {
            HeroesIcons heroesIcons = new HeroesIcons(false);
            List<string> assertMessages = new List<string>();
            int segment = 4;

            foreach (int build in heroesIcons.GetListOfHeroesBuilds())
            {
                heroesIcons.LoadHeroesBuild(build);

                var heroes = heroesIcons.Heroes().GetListOfHeroes(build);

                foreach (var hero in heroes)
                {
                    string altName = heroesIcons.Heroes().GetAltNameFromRealHeroName(hero);

                    if (string.IsNullOrEmpty(altName))
                        Assert.Fail($"[{build}] alt name is null or emtpy for {hero}");

                    BitmapImage heroLeaderBoardPortrait = heroesIcons.Heroes().GetHeroLeaderboardPortrait(hero);
                    BitmapImage heroLoadingPortrait = heroesIcons.Heroes().GetHeroLoadingPortrait(hero);
                    BitmapImage heroPortrait = heroesIcons.Heroes().GetHeroPortrait(hero);

                    if (heroLeaderBoardPortrait.UriSource.Segments[segment].ToString() != $"storm_ui_ingame_hero_leaderboard_{GetUniqueHeroName(altName.ToLower())}.dds")
                        assertMessages.Add($"[{build}] Leaderboard portrait not found for {hero}");

                    if (heroLoadingPortrait.UriSource.Segments[segment].ToString() != $"storm_ui_ingame_hero_loadingscreen_{GetUniqueHeroName(altName.ToLower())}.dds")
                        assertMessages.Add($"[{build}] Loading portrait not found for {hero}");

                    if (heroPortrait.UriSource.Segments[segment].ToString() != $"storm_ui_ingame_heroselect_btn_{GetUniqueHeroName(altName.ToLower())}.dds")
                        assertMessages.Add($"[{build}] Hero portrait not found for {hero}");

                    if (heroesIcons.Heroes().GetHeroFranchise(hero) == HeroFranchise.Unknown)
                        assertMessages.Add($"[{build}] Unknown franchise for {hero}");

                    var heroRoles = heroesIcons.Heroes().GetHeroRoleList(hero);
                    if (heroRoles[0] == HeroRole.Unknown)
                        assertMessages.Add($"[{build}] Unknown hero role for {hero}");
                    if (heroRoles.Count > 1 && heroRoles[0] != HeroRole.Multiclass)
                        assertMessages.Add($"[{build}] Hero {hero} has multiple roles but first role is NOT Multiclass");
                }
            }

            AssertFailMessage(assertMessages);
        }

        [TestMethod]
        public void HeroesBuildTest()
        {
            HeroesIcons heroesIcons = new HeroesIcons(false);
            List<string> assertMessages = new List<string>();
            int segment = 5;

            foreach (int build in heroesIcons.GetListOfHeroesBuilds())
            {
                heroesIcons.LoadHeroesBuild(build);

                var heroes = heroesIcons.Heroes().GetListOfHeroes(build);

                foreach (var hero in heroes)
                {
                    var talents = heroesIcons.HeroBuilds().GetAllTalentsForHero(hero);

                    if (talents == null)
                    {
                        assertMessages.Add($"[{build}] No talents found for {hero}");
                        continue;
                    }
                    if (talents[TalentTier.Level1] == null || talents[TalentTier.Level1].Count < 1)
                        assertMessages.Add($"[{build}] No Level 1 talents for {hero}");
                    if (talents[TalentTier.Level4] == null || talents[TalentTier.Level4].Count < 1)
                        assertMessages.Add($"[{build}] No Level 4 talents for {hero}");
                    if (talents[TalentTier.Level7] == null || talents[TalentTier.Level7].Count < 1)
                        assertMessages.Add($"[{build}] No Level 7 talents for {hero}");
                    if (talents[TalentTier.Level10] == null || talents[TalentTier.Level10].Count < 1)
                        assertMessages.Add($"[{build}] No Level 10 talents for {hero}");
                    if (talents[TalentTier.Level13] == null || talents[TalentTier.Level13].Count < 1)
                        assertMessages.Add($"[{build}] No Level 13 talents for {hero}");
                    if (talents[TalentTier.Level16] == null || talents[TalentTier.Level16].Count < 1)
                        assertMessages.Add($"[{build}] No Level 16 talents for {hero}");
                    if (talents[TalentTier.Level20] == null || talents[TalentTier.Level20].Count < 1)
                        assertMessages.Add($"[{build}] No Level 20 talents for {hero}");

                    // loop through each talent tier
                    foreach (var talentTier in talents)
                    {
                        if (talentTier.Key == TalentTier.Old)
                            continue;

                        // loop through each talent
                        foreach (var talent in talentTier.Value)
                        {
                            BitmapImage talentImage = heroesIcons.HeroBuilds().GetTalentIcon(talent);

                            if (talentImage.UriSource.Segments[segment].ToString() == HeroesBase.NoTalentIconPick || talentImage.UriSource.Segments[segment].ToString() == HeroesBase.NoTalentIconFound)
                                assertMessages.Add($"[{build}] Talent image not found for {talent} [{talentTier.Key.ToString()}]");

                            TalentTooltip talentTooltip = heroesIcons.HeroBuilds().GetTalentTooltips(talent);
                            if (string.IsNullOrEmpty(talentTooltip.Full))
                            {
                                assertMessages.Add($"[{build}] Full tooltip not found for {talent} [{talentTier.Key.ToString()}]");
                            }
                            else
                            {
                                string strippedText = TalentTooltipStripNonText(talentTooltip.Full);

                                if (NonValidCharsCheck(strippedText))
                                    assertMessages.Add($"[{build}] Invalid chars in FULL tooltip {talent} [{talentTier.Key.ToString()}]{Environment.NewLine}{strippedText}{Environment.NewLine}");
                            }

                            if (string.IsNullOrEmpty(talentTooltip.Short))
                            {
                                assertMessages.Add($"[{build}] Short tooltip not found for {talent} [{talentTier.Key.ToString()}]");
                            }
                            else
                            {
                                string strippedText = TalentTooltipStripNonText(talentTooltip.Short);

                                if (NonValidCharsCheck(strippedText))
                                    assertMessages.Add($"[{build}] Invalid chars in SHORT tooltip {talent} [{talentTier.Key.ToString()}]{Environment.NewLine}{strippedText}{Environment.NewLine}");
                            }

                            if (string.IsNullOrEmpty(heroesIcons.HeroBuilds().GetTrueTalentName(talent.Trim())))
                                assertMessages.Add($"[{build}] No true talent name for {talent} [{talentTier.Key.ToString()}]");
                        }
                    }
                }
            }

            AssertFailMessage(assertMessages);
        }

        [TestMethod]
        public void HeroesMapBackgroundsTest()
        {
            HeroesIcons heroesIcons = new HeroesIcons(false);
            List<string> assertMessages = new List<string>();

            Assert.AreEqual(heroesIcons.MapBackgrounds().TotalCountOfMaps(), heroesIcons.MapBackgrounds().GetMapsList().Count, "Number of awards in _AllMapBackgrounds.xml is not equal to number of MapBackgrounds");
            Assert.AreEqual(heroesIcons.MapBackgrounds().TotalCountOfMaps(), Directory.GetFiles($@"Xml\MapBackgrounds").Count() - 1, "Number of maps in _AllMapBackgrounds.xml is not equal to number of files in Xml\\MapBackgrounds");

            foreach (var map in heroesIcons.MapBackgrounds().GetMapsList())
            {
                if (heroesIcons.MapBackgrounds().GetMapBackground(map) == null)
                    assertMessages.Add($"No image found for {map}");
            }
        }

        [TestMethod]
        public void HeroesMatchAwardsTest()
        {
            HeroesIcons heroesIcons = new HeroesIcons(false);
            List<string> assertMessages = new List<string>();

            Assert.AreEqual(heroesIcons.MatchAwards().TotalCountOfAwards(), heroesIcons.MatchAwards().GetMatchAwardsList().Count, "Number of awards in _AllMatchAwards.xml is not equal to number of MatchAwards");
            Assert.AreEqual(heroesIcons.MatchAwards().TotalCountOfAwards(), Directory.GetFiles($@"Xml\MatchAwards").Count() - 1, "Number of awards in _AllMatchAwards.xml is not equal to number of files in Xml\\MatchAwards");

            foreach (var award in heroesIcons.MatchAwards().GetMatchAwardsList())
            {
                if (heroesIcons.MatchAwards().GetMVPScoreScreenAward(award, MVPScoreScreenColor.Blue, out string awardBlueName) == null)
                    assertMessages.Add($"No blue MVP score screen award image found for {award}");

                if (heroesIcons.MatchAwards().GetMVPScoreScreenAward(award, MVPScoreScreenColor.Red, out string awardRedName) == null)
                    assertMessages.Add($"No red MVP score screen award image found for {award}");

                if (heroesIcons.MatchAwards().GetMVPScreenAward(award, MVPScreenColor.Blue, out string awardBlueName2) == null)
                    assertMessages.Add($"No blue MVP screen award image found for {award}");

                if (heroesIcons.MatchAwards().GetMVPScreenAward(award, MVPScreenColor.Red, out string awardRedName2) == null)
                    assertMessages.Add($"No red MVP screen award image found for {award}");

                if (heroesIcons.MatchAwards().GetMVPScreenAward(award, MVPScreenColor.Gold, out string awardGoldName2) == null)
                    assertMessages.Add($"No gold MVP screen award image found for {award}");

                if (string.IsNullOrEmpty(heroesIcons.MatchAwards().GetMatchAwardDescription(award)))
                {
                    assertMessages.Add($"No description found for {award}");
                }
            }
        }

        private string GetUniqueHeroName(string altName)
        {
            // all lowercase
            switch (altName)
            {
                case "brightwing":
                    return "faeriedragon";
                case "cassia":
                    return "d2amazonf";
                case "greymane":
                    return "genngreymane";
                case "kharazim":
                    return "monk";
                case "liming":
                    return "wizard";
                case "ltmorales":
                    return "medic";
                case "nazeebo":
                    return "witchdoctor";
                case "sonya":
                    return "femalebarbarian";
                case "valla":
                    return "demonhunter";
                case "xul":
                    return "necromancer";
                default:
                    return altName;
            }
        }

        // if this algorithm is changed then TalentTooltipTextConverter.cs in Core needs to be changed as well
        private string TalentTooltipStripNonText(string text)
        {
            string strippedText = string.Empty;

            while (text.Length > 0)
            {
                int startIndex = text.IndexOf("<");

                if (startIndex > -1)
                {
                    int endIndex = text.IndexOf(">", startIndex) + 1;

                    // example <c val="#TooltipNumbers">
                    string startTag = text.Substring(startIndex, endIndex - startIndex);

                    if (startTag == "<n/>" || startTag == "</n>")
                    {
                        strippedText += text.Substring(0, startIndex);
                        strippedText += Environment.NewLine;

                        // remove, this part of the string is not needed anymore
                        text = text.Remove(0, endIndex);

                        continue;
                    }
                    else if (startTag.ToLower().StartsWith("<c val="))
                    {
                        int offset = 4;
                        int closingCTagIndex = text.ToLower().IndexOf("</c>", endIndex);

                        // check if an ending tag exists
                        if (closingCTagIndex > 0)
                        {
                            strippedText += text.Substring(0, startIndex);
                            strippedText += text.Substring(endIndex, closingCTagIndex - endIndex);

                            // remove, this part of the string is not needed anymore
                            text = text.Remove(0, closingCTagIndex + offset);
                        }
                        else
                        {
                            strippedText += text.Substring(0, startIndex);

                            // add the rest of the text
                            strippedText += text.Substring(endIndex, text.Length - endIndex);

                            // none left
                            text = string.Empty;
                        }
                    }
                    else if (startTag.StartsWith("<img path=\"@UI/StormTalentInTextQuestIcon\"") || startTag.StartsWith("<img  path=\"@UI/StormTalentInTextQuestIcon\""))
                    {
                        int closingTag = text.IndexOf("/>");

                        strippedText += text.Substring(0, startIndex);

                        // remove, this part of the string is not needed anymore
                        text = text.Remove(0, closingTag + 2);
                    }
                    else
                    {
                        strippedText += text;
                        text = string.Empty;
                    }
                }
                else
                {
                    // add the rest
                    if (text.Length > 0)
                        strippedText += text;

                    text = string.Empty;
                }
            }

            return strippedText;
        }

        private bool NonValidCharsCheck(string text)
        {
            if ((text.IndexOfAny("<>".ToCharArray()) != -1))
                return true;
            else
                return false;
        }

        private void AssertFailMessage(List<string> assertMessages)
        {
            if (assertMessages.Count < 1)
                return;

            string assertMessage = Environment.NewLine;
            foreach (var message in assertMessages)
            {
                assertMessage += $"{message}{Environment.NewLine}";
            }
            
            Assert.Fail(assertMessage);
        }
    }
}
