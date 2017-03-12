using Heroes.Icons;
using HeroesMatchData.Core.Services;
using HeroesMatchData.Data.Models.Replays;

namespace HeroesMatchData.Core.Models.MatchModels
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
            DamageTakenRole = matchPlayerStats.DamageTakenRole;
            HealingRole = matchPlayerStats.HealingRole;
            ExperienceContribution = matchPlayerStats.ExperienceContribution;
            RoleWarrior = matchPlayerStats.RoleWarrior;
            RoleSupport = matchPlayerStats.RoleSupport;
            HighestSiegeDamage = matchPlayerStats.HighestSiegeDamage;
            HighestHeroDamage = matchPlayerStats.HighestHeroDamage;
            HighestExperience = matchPlayerStats.HighestExperience;
            HighestDamageTaken = matchPlayerStats.HighestDamageTaken;
            HighestHealing = matchPlayerStats.HighestHealing;
        }

        public MatchPlayerStats(MatchPlayerBase matchPlayerBase)
            : base(matchPlayerBase)
        { }

        public MatchPlayerStats(IInternalService internalService, IWebsiteService website, ReplayMatchPlayer player)
            : base(internalService, website, player)
        { }

        public int? SoloKills { get; private set; }
        public int? Assists { get; private set; }
        public int? Deaths { get; private set; }
        public int? SiegeDamage { get; private set; }
        public int? HeroDamage { get; private set; }
        public int? DamageTakenRole { get; private set; }
        public int? HealingRole { get; private set; }
        public int? ExperienceContribution { get; private set; }
        public bool RoleWarrior { get; private set; }
        public bool RoleSupport { get; private set; }
        public bool HighestSiegeDamage { get; set; }
        public bool HighestHeroDamage { get; set; }
        public bool HighestExperience { get; set; }
        public bool HighestDamageTaken { get; set; }
        public bool HighestHealing { get; set; }

        public void SetStats(ReplayMatchPlayerScoreResult playerScore, ReplayMatchPlayer player)
        {
            SoloKills = playerScore.SoloKills;
            Assists = playerScore.Assists;
            Deaths = playerScore.Deaths;
            SiegeDamage = playerScore.SiegeDamage;
            HeroDamage = playerScore.HeroDamage;
            ExperienceContribution = playerScore.ExperienceContribution;

            RoleWarrior = false;
            RoleSupport = false;

            if (playerScore.DamageTaken != null)
            {
                DamageTakenRole = playerScore.DamageTaken;
                RoleWarrior = true;
            }
            else if (IsHealingStatCharacter(player.Character))
            {
                HealingRole = playerScore.Healing;
                RoleSupport = true;
            }
        }

        private bool IsHealingStatCharacter(string realHeroName)
        {
            if (HeroesIcons.Heroes().GetHeroRoleList(realHeroName)[0] == HeroRole.Support || HeroesIcons.IsNonSupportHeroWithHealingStat(realHeroName))
                return true;
            else
                return false;
        }
    }
}
