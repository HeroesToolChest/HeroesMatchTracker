using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Heroes.Helpers.Tests
{
    [TestClass]
    public class EnumParserTests
    {
        [TestMethod]
        public void SeasonTest()
        {
            try
            {
                foreach (Season season in Enum.GetValues(typeof(Season)))
                {
                    HeroesHelpers.EnumParser.ConvertSeasonStringToEnum(HeroesHelpers.Seasons.GetStringFromSeason(season));
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
