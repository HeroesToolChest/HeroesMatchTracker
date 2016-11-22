using HeroesIcons;
using HeroesParserData.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HeroesParserData.ViewModels.Match
{
    public class MatchContextBase : ViewModelBase
    {
        protected readonly Color Team1BackColor = Color.FromRgb(179, 179, 255);
        protected readonly Color Team2BackColor = Color.FromRgb(235, 159, 159);
        protected readonly Color WinningTeamBackColor = Color.FromRgb(233, 252, 233);
        protected readonly Color LosingTeamBackColor = Colors.AliceBlue;

        protected Dictionary<int, PartyIconColor> PlayerPartyIcons { get; private set; } = new Dictionary<int, PartyIconColor>();

        protected bool ShowPlayerTagColumn
        {
            get { return !UserSettings.Default.IsBattleTagHidden; }
            set
            {
                UserSettings.Default.IsBattleTagHidden = !value;
                RaisePropertyChangedEvent(nameof(ShowPlayerTagColumn));
            }
        }

        protected MatchContextBase()
            :base()
        {

        }

        protected void FindPlayerParties(List<ReplayMatchPlayer> playersList)
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

            PlayerPartyIcons = new Dictionary<int, PartyIconColor>();
            PartyIconColor color = 0;

            foreach (var party in parties)
            {
                foreach (int playerNum in party.Value)
                {
                    PlayerPartyIcons.Add(playerNum, color);
                }
                color++;
            }
        }
    }
}
