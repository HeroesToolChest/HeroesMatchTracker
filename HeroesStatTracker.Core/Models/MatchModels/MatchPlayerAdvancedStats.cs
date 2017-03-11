using Heroes.Helpers;
using HeroesStatTracker.Core.Services;
using HeroesStatTracker.Data.Models.Replays;
using System;

namespace HeroesStatTracker.Core.Models.MatchModels
{
    public class MatchPlayerAdvancedStats : MatchPlayerStats
    {
        public MatchPlayerAdvancedStats(MatchPlayerStats matchPlayerStats)
            : base(matchPlayerStats)
        { }

        public MatchPlayerAdvancedStats(IInternalService internalService, IWebsiteService website, ReplayMatchPlayer player)
            : base(internalService, website, player)
        { }

        public string PlayerNameOnly { get; private set; }
        public int? TakeDowns { get; private set; }
        public int? CreepDamage { get; private set; }
        public int? MinionDamage { get; private set; }
        public int? SummonDamage { get; private set; }
        public int? StrutureDamage { get; private set; }
        public int? SelfHealing { get; private set; }
        public int? MercCampCaptures { get; private set; }
        public int? WatchTowerCaptures { get; private set; }
        public TimeSpan? TimeSpentDead { get; private set; }

        public void SetAdvancedStats(ReplayMatchPlayerScoreResult playerScore, ReplayMatchPlayer player)
        {
            PlayerNameOnly = HeroesHelpers.BattleTags.GetNameFromBattleTagName(PlayerName);
            TakeDowns = playerScore.TakeDowns;
            CreepDamage = playerScore.CreepDamage;
            MinionDamage = playerScore.MinionDamage;
            SummonDamage = playerScore.SummonDamage;
            StrutureDamage = playerScore.StructureDamage;
            SelfHealing = playerScore.SelfHealing;
            MercCampCaptures = playerScore.MercCampCaptures;
            WatchTowerCaptures = playerScore.WatchTowerCaptures;
            TimeSpentDead = playerScore.TimeSpentDead;
        }
    }
}
