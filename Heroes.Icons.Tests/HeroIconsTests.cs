using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Heroes.Icons.Tests
{
    [TestClass]
    // these test will normally check the latest build only
    public class HeroIconsTests
    {
        private HeroesIcons HeroesIcons;

        public HeroIconsTests()
        {
            FrameworkElement frameworkElement = new FrameworkElement(); // needed to load up the framework
            HeroesIcons = new HeroesIcons(false);
        }

        [TestMethod]
        public void HeroesTest()
        {
            List<string> assertMessages = new List<string>();
            int segment = 4;
            var heroes = HeroesIcons.Heroes().GetListOfHeroes();

            foreach (var hero in heroes)
            {
                string altName = HeroesIcons.Heroes().GetAltNameFromRealHeroName(hero);

                if (string.IsNullOrEmpty(altName))
                    Assert.Fail($"alt name is null or emtpy for {hero}");

                BitmapImage heroLeaderBoardPortrait = HeroesIcons.Heroes().GetHeroLeaderboardPortrait(hero);
                BitmapImage heroLoadingPortrait = HeroesIcons.Heroes().GetHeroLoadingPortrait(hero);
                BitmapImage heroPortrait = HeroesIcons.Heroes().GetHeroPortrait(hero);


                if (heroLeaderBoardPortrait.UriSource.Segments[segment].ToString() != $"storm_ui_ingame_hero_leaderboard_{GetUniqueHeroName(altName.ToLower())}.dds")
                    assertMessages.Add($"Leaderboard portrait not found for {hero}");

                if (heroLoadingPortrait.UriSource.Segments[segment].ToString() != $"storm_ui_ingame_hero_loadingscreen_{GetUniqueHeroName(altName.ToLower())}.dds")
                    assertMessages.Add($"Loading portrait not found for {hero}");

                if (heroPortrait.UriSource.Segments[segment].ToString() != $"storm_ui_ingame_heroselect_btn_{GetUniqueHeroName(altName.ToLower())}.dds")
                    assertMessages.Add($"Hero portrait not found for {hero}");

                if (HeroesIcons.Heroes().GetHeroFranchise(hero) == HeroFranchise.Unknown)
                    assertMessages.Add($"Unknown franchise for {hero}");

                var heroRoles = HeroesIcons.Heroes().GetHeroRoleList(hero);
                if (heroRoles[0] == HeroRole.Unknown)
                    assertMessages.Add($"Unknown hero role for {hero}");
                if (heroRoles.Count > 1 && heroRoles[0] != HeroRole.Multiclass)
                    assertMessages.Add($"Hero {hero} has multiple roles but first role is NOT Multiclass");          
            }

            AssertFailMessage(assertMessages);
        }

        [TestMethod]
        public void HeroesBuildTest()
        {
            List<string> assertMessages = new List<string>();
            int segment = 5;
            var heroes = HeroesIcons.Heroes().GetListOfHeroes();

            foreach (var hero in heroes)
            {
                var talents = HeroesIcons.HeroBuilds().GetAllTalentsForHero(hero);
                if (talents[TalentTier.Level1].Count < 1)
                    Assert.Fail($"No Level 1 talents for {hero}");
                if (talents[TalentTier.Level4].Count < 1)
                    Assert.Fail($"No Level 4 talents for {hero}");
                if (talents[TalentTier.Level7].Count < 1)
                    Assert.Fail($"No Level 7 talents for {hero}");
                if (talents[TalentTier.Level10].Count < 1)
                    Assert.Fail($"No Level 10 talents for {hero}");
                if (talents[TalentTier.Level13].Count < 1)
                    Assert.Fail($"No Level 13 talents for {hero}");
                if (talents[TalentTier.Level16].Count < 1)
                    Assert.Fail($"No Level 16 talents for {hero}");
                if (talents[TalentTier.Level20].Count < 1)
                    Assert.Fail($"No Level 20 talents for {hero}");

                // loop through each talent tier
                foreach (var talentTier in talents)
                {
                    if (talentTier.Key == TalentTier.Old)
                        continue;

                    // loop through each talent
                    foreach (var talent in talentTier.Value)
                    {
                        BitmapImage talentImage = HeroesIcons.HeroBuilds().GetTalentIcon(talent);

                        if (talentImage.UriSource.Segments[segment].ToString() == HeroesBase.NoTalentIconPick || talentImage.UriSource.Segments[segment].ToString() == HeroesBase.NoTalentIconFound)
                            assertMessages.Add($"Talent image not found for {talent} [{talentTier.Key.ToString()}]");

                        TalentTooltip talentTooltip = HeroesIcons.HeroBuilds().GetTalentTooltips(talent);
                        if (string.IsNullOrEmpty(talentTooltip.Full))
                        {
                            assertMessages.Add($"Full tooltip not found for {talent} [{talentTier.Key.ToString()}]");
                        }
                        else
                        {
                            string strippedText = TalentTooltipStripNonText(talentTooltip.Full);

                            if (NonValidCharsCheck(strippedText))
                                assertMessages.Add($"Invalid chars in FULL tooltip {talent} [{talentTier.Key.ToString()}]{Environment.NewLine}{strippedText}{Environment.NewLine}");
                        }

                        if (string.IsNullOrEmpty(talentTooltip.Short))
                        {
                            assertMessages.Add($"Short tooltip not found for {talent} [{talentTier.Key.ToString()}]");
                        }
                        else
                        {
                            string strippedText = TalentTooltipStripNonText(talentTooltip.Short);

                            if (NonValidCharsCheck(strippedText))
                                assertMessages.Add($"Invalid chars in SHORT tooltip {talent} [{talentTier.Key.ToString()}]{Environment.NewLine}{strippedText}{Environment.NewLine}");
                        }

                        if (string.IsNullOrEmpty(HeroesIcons.HeroBuilds().GetTrueTalentName(talent.Trim())))
                            assertMessages.Add($"No true talent name for {talent} [{talentTier.Key.ToString()}]");
                    }
                }             
            }

            AssertFailMessage(assertMessages);
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

        // if this algorithm is changed then TalentTooltipTextConverter.cs in Core needs to be chagned as well
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
                        string colorValue = startTag.Substring(8, startTag.Length - 10);

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
