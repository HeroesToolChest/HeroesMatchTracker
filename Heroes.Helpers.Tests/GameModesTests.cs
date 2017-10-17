using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Heroes.Helpers.Tests
{
    [TestClass]
    public class GameModesTests
    {
        [TestMethod]
        public void GameModeListTests()
        {
            var gameModesList = HeroesHelpers.GameModes.GetGameModesList();

            foreach (string gameMode in gameModesList)
            {
                try
                {
                    gameMode.ConvertToEnum<GameMode>();
                }
                catch (ArgumentException)
                {
                    Assert.Fail($"Failed to convert game mode string [{gameMode}] to GameMode enum");
                }
            }
        }
    }
}
