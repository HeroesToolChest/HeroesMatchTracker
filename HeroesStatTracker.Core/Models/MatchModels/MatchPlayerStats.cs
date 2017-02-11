using Heroes.Icons;
using HeroesStatTracker.Data;
using HeroesStatTracker.Data.Models.Replays;

namespace HeroesStatTracker.Core.Models.MatchModels
{
    public class MatchPlayerStats : MatchPlayerBase
    {
        public MatchPlayerStats(MatchPlayerBase matchPlayerBase)
            : base(matchPlayerBase)
        { }

        public MatchPlayerStats(IDatabaseService database, IHeroesIconsService heroesIcons, ReplayMatchPlayer player)
            : base(database, heroesIcons, player)
        { }

        public int? SoloKills { get; set; }
        public int? Assists { get; set; }
        public int? Deaths { get; set; }
        public int? SiegeDamage { get; set; }
        public int? HeroDamage { get; set; }
        public int? Role { get; set; }
        public int? ExperienceContribution { get; set; }
        public bool RoleWarrior { get; set; }
        public bool RoleSupport { get; set; }

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
                Role = playerScore.DamageTaken;
                RoleWarrior = true;
            }
            else if (IsHealingStatCharacter(player.Character))
            {
                Role = playerScore.Healing;
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
