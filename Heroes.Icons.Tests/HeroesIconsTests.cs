using Heroes.Icons.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Heroes.Icons.Tests
{
    [TestClass]
    public class HeroesIconsTests : HeroesIconsBase
    {
        [TestMethod]
        public void GetOtherIconTests()
        {
            List<string> assertMessages = new List<string>();

            foreach (OtherIcon icon in Enum.GetValues(typeof(OtherIcon)))
            {
                if (HeroesIcons.GetOtherIcon(icon) == null)
                    assertMessages.Add($"Other icon {icon} is null");
            }

            AssertFailMessage(assertMessages);
        }

        [TestMethod]
        public void GetPartyIconTests()
        {
            List<string> assertMessages = new List<string>();

            foreach (PartyIconColor icon in Enum.GetValues(typeof(PartyIconColor)))
            {
                if (HeroesIcons.GetPartyIcon(icon) == null)
                    assertMessages.Add($"Party icon {icon} is null");
            }

            AssertFailMessage(assertMessages);
        }

        [TestMethod]
        public void GetRoleIconTests()
        {
            List<string> assertMessages = new List<string>();

            foreach (HeroRole icon in Enum.GetValues(typeof(HeroRole)))
            {
                if (icon == HeroRole.Multiclass)
                    continue;

                if (HeroesIcons.GetRoleIcon(icon) == null)
                    assertMessages.Add($"Role icon {icon} is null");
            }

            AssertFailMessage(assertMessages);
        }

        [TestMethod]
        public void GetFranchiseIconTests()
        {
            List<string> assertMessages = new List<string>();

            foreach (HeroFranchise icon in Enum.GetValues(typeof(HeroFranchise)))
            {
                if (HeroesIcons.GetFranchiseIcon(icon) == null)
                    assertMessages.Add($"Franchise icon {icon} is null");
            }

            AssertFailMessage(assertMessages);
        }
    }
}
