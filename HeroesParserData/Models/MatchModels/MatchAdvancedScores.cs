using System;

namespace HeroesParserData.Models.MatchModels
{
    public class MatchAdvancedScores : MatchScores
    {
        public int? TakeDowns { get; set; }
        public int? CreepDamage { get; set; }
        public int? MinionDamage { get; set; }
        public int? SummonDamage { get; set; }
        public int? StrutureDamage { get; set; }
        public int? DamageTaken { get; set; }
        public int? Healing { get; set; }
        public int? SelfHealing { get; set; }
        public int? MercCampCaptures { get; set; }
        public int? WatchTowerCaptures { get; set; }
        public TimeSpan? TimeSpentDead { get; set; }

        public MatchAdvancedScores(MatchPlayerInfoBase matchPlayerInfoBase)
            :base(matchPlayerInfoBase)
        {
            
        }
    }
}
