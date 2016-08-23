using System;

namespace HeroesParserData.Models.MatchModels
{
    public class MatchChat
    {
        public TimeSpan TimeStamp { get; set; }
        public string Target { get; set; }
        public string ChatMessage { get; set; }
    }
}
