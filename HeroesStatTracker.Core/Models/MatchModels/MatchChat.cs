using HeroesStatTracker.Data.Models.Replays;
using System;

namespace HeroesStatTracker.Core.Models.MatchModels
{
    public class MatchChat
    {
        public TimeSpan TimeStamp { get; private set; }
        public string Target { get; private set; }
        public string ChatMessage { get; private set; }

        public void SetChatMessages(ReplayMatchMessage matchMessage)
        {
            TimeStamp = matchMessage.TimeStamp;
            Target = matchMessage.MessageTarget;

            if (string.IsNullOrEmpty(matchMessage.PlayerName))
                ChatMessage = $"((Unknown)): {matchMessage.Message}";
            else if (!string.IsNullOrEmpty(matchMessage.CharacterName))
                ChatMessage = $"{matchMessage.PlayerName} ({matchMessage.CharacterName}): {matchMessage.Message}";
            else
                ChatMessage = $"{matchMessage.PlayerName}: {matchMessage.Message}";
        }
    }
}
