using HeroesParserData.Models.DbModels;
using System.Collections.Generic;

namespace HeroesParserData.Messages
{
    public class MatchSummaryMessage
    {
        public Replay Replay { get; set; }
        public MatchSummary MatchSummary { get; set; }
        public List<Replay> MatchList { get; set; }
    }
}
