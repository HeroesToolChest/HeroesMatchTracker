using Heroes.Icons.Models;
using HeroesMatchTracker.Data.Models.Replays;
using System;
using System.Collections.Generic;

namespace HeroesMatchTracker.Core
{
    public class PlayerParties
    {
        public static Dictionary<int, PartyIconColor> FindPlayerParties(ICollection<ReplayMatchPlayer> playersList)
        {
            Dictionary<long, List<Tuple<int, int?>>> parties = new Dictionary<long, List<Tuple<int, int?>>>();

            foreach (var player in playersList)
            {
                if (player.PartyValue != 0)
                {
                    if (!parties.ContainsKey(player.PartyValue))
                    {
                        var partyPlayer = new Tuple<int, int?>(player.PlayerNumber, player.Team);
                        var listOfMembers = new List<Tuple<int, int?>>
                        {
                            partyPlayer,
                        };

                        parties.Add(player.PartyValue, listOfMembers);
                    }
                    else
                    {
                        var listOfMembers = parties[player.PartyValue];
                        listOfMembers.Add(new Tuple<int, int?>(player.PlayerNumber, player.Team));
                        parties[player.PartyValue] = listOfMembers;
                    }
                }
            }

            var playerPartyIcons = new Dictionary<int, PartyIconColor>();

            var team0Color = PartyIconColor.Purple;
            var team1Color = PartyIconColor.Red;
            bool purpleUsed = false;
            bool redUsed = false;

            foreach (var party in parties)
            {
                foreach (var player in party.Value)
                {
                    if (player.Item2 == 0)
                    {
                        playerPartyIcons.Add(player.Item1, team0Color);
                        purpleUsed = true;
                    }
                    else if (player.Item2 == 1)
                    {
                        playerPartyIcons.Add(player.Item1, team1Color);
                        redUsed = true;
                    }
                }

                if (purpleUsed)
                    team0Color = PartyIconColor.Blue;
                if (redUsed)
                    team1Color = PartyIconColor.Orange;
            }

            return playerPartyIcons;
        }
    }
}
