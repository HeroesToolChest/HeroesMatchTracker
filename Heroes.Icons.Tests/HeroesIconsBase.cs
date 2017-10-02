using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Heroes.Icons.Tests
{
    //public class HeroesIconsBase
    //{
    //    public const string NoPortraitPick = "storm_ui_ingame_heroselect_btn_nopick.dds";
    //    public const string NoPortraitFound = "storm_ui_ingame_heroselect_btn_notfound.dds";
    //    public const string NoLoadingScreenPick = "storm_ui_ingame_hero_loadingscreen_nopick.dds";
    //    public const string NoLoadingScreenFound = "storm_ui_ingame_hero_loadingscreen_notfound.dds";
    //    public const string NoLeaderboardPick = "storm_ui_ingame_hero_leaderboard_nopick.dds";
    //    public const string NoLeaderboardFound = "storm_ui_ingame_hero_leaderboard_notfound.dds";
    //    public const string NoTalentIconPick = "storm_ui_ingame_leader_talent_unselected.png";
    //    public const string NoTalentIconFound = "storm_ui_icon_default.dds";

    //    public HeroesIconsBase()
    //    {
    //        FrameworkElement frameworkElement = new FrameworkElement(); // needed to load up the framework
    //        HeroesIcons = new HeroesIcons(false);
    //    }

    //    protected static string ImageMissingLogName => "_ImageMissingLog.txt";
    //    protected static string ReferenceLogName => "_ReferenceNameLog.txt";
    //    protected static string XmlErrorsLogName => "_XmlErrorsLog.txt";
    //    protected static string LogFileName => "Logs";
    //    protected static string ApplicationIconsPath => "pack://application:,,,/Heroes.Icons;component/Icons";
    //    protected HeroesIcons HeroesIcons { get; set; }

    //    protected bool NonValidCharsCheck(string text)
    //    {
    //        if (text.IndexOfAny("<>".ToCharArray()) != -1)
    //            return true;
    //        else
    //            return false;
    //    }

    //    protected void AssertFailMessage(List<string> assertMessages)
    //    {
    //        if (assertMessages.Count < 1)
    //            return;

    //        string assertMessage = Environment.NewLine;
    //        foreach (var message in assertMessages)
    //        {
    //            assertMessage += $"{message}{Environment.NewLine}";
    //        }

    //        Assert.Fail(assertMessage);
    //    }

    //    protected string GetUniqueHeroName(string altName)
    //    {
    //        // all lowercase
    //        switch (altName)
    //        {
    //            case "brightwing":
    //                return "faeriedragon";
    //            case "cassia":
    //                return "d2amazonf";
    //            case "greymane":
    //                return "genngreymane";
    //            case "kharazim":
    //                return "monk";
    //            case "liming":
    //                return "wizard";
    //            case "ltmorales":
    //                return "medic";
    //            case "nazeebo":
    //                return "witchdoctor";
    //            case "sonya":
    //                return "femalebarbarian";
    //            case "valla":
    //                return "demonhunter";
    //            case "xul":
    //                return "necromancer";
    //            default:
    //                return altName;
    //        }
    //    }

    //    // if this algorithm is changed then TalentTooltipTextConverter.cs in Core needs to be changed as well
    //    protected string TalentTooltipStripNonText(string text)
    //    {
    //        string strippedText = string.Empty;

    //        while (text.Length > 0)
    //        {
    //            int startIndex = text.IndexOf("<");

    //            if (startIndex > -1)
    //            {
    //                int endIndex = text.IndexOf(">", startIndex) + 1;

    //                // example <c val="#TooltipNumbers">
    //                string startTag = text.Substring(startIndex, endIndex - startIndex);

    //                if (startTag == "<n/>" || startTag == "</n>")
    //                {
    //                    strippedText += text.Substring(0, startIndex);
    //                    strippedText += Environment.NewLine;

    //                    // remove, this part of the string is not needed anymore
    //                    text = text.Remove(0, endIndex);

    //                    continue;
    //                }
    //                else if (startTag.ToLower().StartsWith("<c val="))
    //                {
    //                    int offset = 4;
    //                    int closingCTagIndex = text.ToLower().IndexOf("</c>", endIndex);

    //                    // check if an ending tag exists
    //                    if (closingCTagIndex > 0)
    //                    {
    //                        strippedText += text.Substring(0, startIndex);
    //                        strippedText += text.Substring(endIndex, closingCTagIndex - endIndex);

    //                        // remove, this part of the string is not needed anymore
    //                        text = text.Remove(0, closingCTagIndex + offset);
    //                    }
    //                    else
    //                    {
    //                        strippedText += text.Substring(0, startIndex);

    //                        // add the rest of the text
    //                        strippedText += text.Substring(endIndex, text.Length - endIndex);

    //                        // none left
    //                        text = string.Empty;
    //                    }
    //                }
    //                else if (startTag.StartsWith("<img path=\"@UI/StormTalentInTextQuestIcon\"") || startTag.StartsWith("<img  path=\"@UI/StormTalentInTextQuestIcon\""))
    //                {
    //                    int closingTag = text.IndexOf("/>");

    //                    strippedText += text.Substring(0, startIndex);

    //                    // remove, this part of the string is not needed anymore
    //                    text = text.Remove(0, closingTag + 2);
    //                }
    //                else if (startTag.StartsWith("<img path=\"@UI/StormTalentInTextArmorIcon\" alignment=\"uppermiddle\""))
    //                {
    //                    int closingTag = text.IndexOf("/>");

    //                    strippedText += text.Substring(0, startIndex);

    //                    // remove, this part of the string is not needed anymore
    //                    text = text.Remove(0, closingTag + 2);
    //                }
    //                else
    //                {
    //                    strippedText += text;
    //                    text = string.Empty;
    //                }
    //            }
    //            else
    //            {
    //                // add the rest
    //                if (text.Length > 0)
    //                    strippedText += text;

    //                text = string.Empty;
    //            }
    //        }

    //        return strippedText;
    //    }
    //}
}
