using Heroes.Icons;
using HeroesMatchData.Data.Models.Replays;
using System.Collections.Generic;

namespace HeroesMatchData.Core
{
    public class PlayerParties
    {
        public static Dictionary<int, PartyIconColor> FindPlayerParties(ICollection<ReplayMatchPlayer> playersList)
        {
            Dictionary<long, List<int>> parties = new Dictionary<long, List<int>>();

            foreach (var player in playersList)
            {
                if (player.PartyValue != 0)
                {
                    if (!parties.ContainsKey(player.PartyValue))
                    {
                        var listOfMembers = new List<int>();
                        listOfMembers.Add(player.PlayerNumber);
                        parties.Add(player.PartyValue, listOfMembers);
                    }
                    else
                    {
                        var listOfMembers = parties[player.PartyValue];
                        listOfMembers.Add(player.PlayerNumber);
                        parties[player.PartyValue] = listOfMembers;
                    }
                }
            }

            var playerPartyIcons = new Dictionary<int, PartyIconColor>();
            PartyIconColor color = 0;

            foreach (var party in parties)
            {
                foreach (int playerNum in party.Value)
                {
                    playerPartyIcons.Add(playerNum, color);
                }

                color++;
            }

            return playerPartyIcons;
        }
    }
}
