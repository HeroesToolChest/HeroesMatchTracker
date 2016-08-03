using HeroesIcons;
using HeroesParserData.DataQueries.ReplayData;
using HeroesParserData.Models;
using HeroesParserData.Models.DbModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HeroesParserData.ViewModels.Match
{
    public class LastGameViewModel : MatchContext
    {
        private ObservableCollection<MatchInfo> _matchInfoTeam1 = new ObservableCollection<MatchInfo>();
        private ObservableCollection<MatchInfo> _matchInfoTeam2 = new ObservableCollection<MatchInfo>();
        private TalentIcons TalentIcons = new TalentIcons();

        public ObservableCollection<MatchInfo> MatchInfoTeam1
        {
            get
            {
                return _matchInfoTeam1;
            }
            set
            {
                _matchInfoTeam1 = value;
                RaisePropertyChangedEvent("MatchInfoTeam1");
            }
        }

        public ObservableCollection<MatchInfo> MatchInfoTeam2
        {
            get
            {
                return _matchInfoTeam2;
            }
            set
            {
                _matchInfoTeam2 = value;
                RaisePropertyChangedEvent("MatchInfoTeam2");
            }
        }

        public ICommand Refresh
        {
            get { return new DelegateCommand(async () => await QueryGameDetails()); }
        }

        public LastGameViewModel()
        {
        }

        private async Task QueryGameDetails()
        {
            Replay replay = (await Query.Replay.ReadLastRecordsAsync(1))[0];

            replay = await Query.Replay.ReadReplayRecord(replay.ReplayId);

            List<ReplayMatchPlayer> players = await Query.MatchPlayer.ReadRecordsByReplayId(replay.ReplayId);
            List<ReplayMatchPlayerTalent> playerTalents = await Query.MatchPlayerTalent.ReadRecordsByReplayId(replay.ReplayId);

            foreach (var player in players)
            {
                MatchInfo matchinfo = new MatchInfo();

                matchinfo.PlayerName = Utilities.GetNameFromBattleTagName((await Query.HotsPlayer.ReadRecordFromPlayerId(player.PlayerId)).BattleTagName);
                matchinfo.CharacterName = player.Character;
                matchinfo.CharacterLevel = player.CharacterLevel;
                matchinfo.Talent1 = TalentIcons.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName1);
                matchinfo.Talent4 = TalentIcons.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName2);
                matchinfo.Talent7 = TalentIcons.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName3);
                matchinfo.Talent10 = TalentIcons.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName4);
                matchinfo.Talent13 = TalentIcons.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName5);
                matchinfo.Talent16 = TalentIcons.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName6);
                matchinfo.Talent20 = TalentIcons.GetTalentIcon(playerTalents[player.PlayerNumber].TalentName7);

                if (player.Team == 0)
                    MatchInfoTeam1.Add(matchinfo);
                else if (player.Team == 1)
                    MatchInfoTeam2.Add(matchinfo);
                //else 
                    // observer

            }                 
        }
    }
}
