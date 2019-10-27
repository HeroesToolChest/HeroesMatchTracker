using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Data.Models.Replays;

namespace HeroesMatchTracker.Core.Models.MatchModels
{
    public class MatchPlayerStats : MatchPlayerBase
    {
        public MatchPlayerStats(MatchPlayerStats matchPlayerStats)
            : base(matchPlayerStats)
        {
            SoloKills = matchPlayerStats.SoloKills;
            Assists = matchPlayerStats.Assists;
            Deaths = matchPlayerStats.Deaths;
            SiegeDamage = matchPlayerStats.SiegeDamage;
            HeroDamage = matchPlayerStats.HeroDamage;
            DamageTaken = matchPlayerStats.DamageTaken;
            HealingRole = matchPlayerStats.HealingRole;
            SelfHealing = matchPlayerStats.SelfHealing;
            ExperienceContribution = matchPlayerStats.ExperienceContribution;
            RoleWarrior = matchPlayerStats.RoleWarrior;
            RoleSupport = matchPlayerStats.RoleSupport;
            HighestMinionDamage = matchPlayerStats.HighestMinionDamage;
            HighestSummonDamage = matchPlayerStats.HighestSummonDamage;
            HighestStructureDamage = matchPlayerStats.HighestStructureDamage;
            HighestSiegeDamage = matchPlayerStats.HighestSiegeDamage;
            HighestHeroDamage = matchPlayerStats.HighestHeroDamage;
            HighestExperience = matchPlayerStats.HighestExperience;
            HighestDamageTaken = matchPlayerStats.HighestDamageTaken;
            HighestHealing = matchPlayerStats.HighestHealing;
            HighestSelfHealing = matchPlayerStats.HighestSelfHealing;
            HighestLiveTime = matchPlayerStats.HighestLiveTime;
            HighestMercDamage = matchPlayerStats.HighestMercDamage;
            HighestMercCaptures = matchPlayerStats.HighestMercCaptures;
            HighestWatchTowerCaptures = matchPlayerStats.HighestWatchTowerCaptures;
            HighestKills = matchPlayerStats.HighestKills;
            HighestTakedowns = matchPlayerStats.HighestTakedowns;
            HighestAssists = matchPlayerStats.HighestAssists;
            HighestNonDeaths = matchPlayerStats.HighestNonDeaths;
            HighestTownKills = matchPlayerStats.HighestTownKills;
            HighestSpellDamage = matchPlayerStats.HighestSpellDamage;
            HighestPhysicalDamage = matchPlayerStats.HighestPhysicalDamage;
        }

        public MatchPlayerStats(MatchPlayerBase matchPlayerBase)
            : base(matchPlayerBase)
        { }

        public MatchPlayerStats(IInternalService internalService, IWebsiteService website, ReplayMatchPlayer player, int build)
            : base(internalService, website, player, build)
        { }

        public int? SoloKills { get; private set; }
        public int? Assists { get; private set; }
        public int? Deaths { get; private set; }
        public int? SiegeDamage { get; private set; }
        public int? HeroDamage { get; private set; }
        public int? DamageTaken { get; private set; }
        public int? HealingRole { get; private set; }
        public int? SelfHealing { get; private set; }
        public int? ExperienceContribution { get; private set; }
        public bool RoleWarrior { get; private set; }
        public bool RoleSupport { get; private set; }
        public bool HighestMinionDamage { get; set; }
        public bool HighestSummonDamage { get; set; }
        public bool HighestStructureDamage { get; set; }
        public bool HighestSiegeDamage { get; set; }
        public bool HighestHeroDamage { get; set; }
        public bool HighestExperience { get; set; }
        public bool HighestDamageTaken { get; set; }
        public bool HighestHealing { get; set; }
        public bool HighestSelfHealing { get; set; }
        public bool HighestLiveTime { get; set; }
        public bool HighestMercDamage { get; set; }
        public bool HighestMercCaptures { get; set; }
        public bool HighestWatchTowerCaptures { get; set; }
        public bool HighestKills { get; set; }
        public bool HighestTakedowns { get; set; }
        public bool HighestAssists { get; set; }
        public bool HighestNonDeaths { get; set; }
        public bool HighestTownKills { get; set; }
        public bool HighestSpellDamage { get; set; }
        public bool HighestPhysicalDamage { get; set; }

        public void SetStats(ReplayMatchPlayerScoreResult playerScore, ReplayMatchPlayer player)
        {
            SoloKills = playerScore.SoloKills;
            Assists = playerScore.Assists;
            Deaths = playerScore.Deaths;
            SiegeDamage = playerScore.SiegeDamage;
            HeroDamage = playerScore.HeroDamage;
            HealingRole = playerScore.Healing;
            SelfHealing = playerScore.SelfHealing;
            DamageTaken = playerScore.DamageTaken;
            ExperienceContribution = playerScore.ExperienceContribution;
        }
    }
}
