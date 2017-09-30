using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Heroes.Helpers.Tests
{
    [TestClass]
    public class GameDateFiltersTests
    {
        [TestMethod]
        public void GetGameTimeModifiedTimeTest()
        {
            foreach (FilterGameTimeOption option in Enum.GetValues(typeof(FilterGameTimeOption)))
            {
                if (option != FilterGameTimeOption.Any)
                    Assert.IsNotNull(HeroesHelpers.GameDateFilters.GetGameTimeModifiedTime(option).Item1);
                else
                    Assert.IsNull(HeroesHelpers.GameDateFilters.GetGameTimeModifiedTime(option).Item1);
            }
        }

        [TestMethod]
        public void GetGameDateModifiedDateTest()
        {
            foreach (FilterGameDateOption option in Enum.GetValues(typeof(FilterGameDateOption)))
            {
                if (option != FilterGameDateOption.Any)
                    Assert.IsNotNull(HeroesHelpers.GameDateFilters.GetGameDateModifiedDate(option).Item1);
                else
                    Assert.IsNull(HeroesHelpers.GameDateFilters.GetGameDateModifiedDate(option).Item1);
            }
        }
    }
}
