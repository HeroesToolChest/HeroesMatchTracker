using HeroesIcons;
using HeroesParserData.DataQueries.ReplayData;
using HeroesParserData.Models;
using HeroesParserData.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HeroesParserData.ViewModels.Match
{
    public class LastGameViewModel : MatchContext
    {
        //private ObservableCollection<MatchInfo> _matchInfoTeam1 = new ObservableCollection<MatchInfo>();
        //private ObservableCollection<MatchInfo> _matchInfoTeam2 = new ObservableCollection<MatchInfo>();
        //private long _replayId;
        //private string _gameMode;
        //private string _mapName;
        //private DateTime? _gameDate;
        //private TimeSpan _gameTime;

        //private TalentIcons TalentIcons = new TalentIcons();

        //public ObservableCollection<MatchInfo> MatchInfoTeam1
        //{
        //    get { return _matchInfoTeam1; }
        //    set
        //    {
        //        _matchInfoTeam1 = value;
        //        RaisePropertyChangedEvent("MatchInfoTeam1");
        //    }
        //}

        //public ObservableCollection<MatchInfo> MatchInfoTeam2
        //{
        //    get { return _matchInfoTeam2; }
        //    set
        //    {
        //        _matchInfoTeam2 = value;
        //        RaisePropertyChangedEvent("MatchInfoTeam2");
        //    }
        //}

        //public long ReplayId
        //{
        //    get { return _replayId; }
        //    set
        //    {
        //        _replayId = value;
        //        RaisePropertyChangedEvent("ReplayId");
        //    }
        //}

        //public string GameMode
        //{
        //    get { return _gameMode; }
        //    set
        //    {
        //        _gameMode = value;
        //        RaisePropertyChangedEvent("GameMode");
        //    }
        //}

        //public string MapName
        //{
        //    get { return _mapName; }
        //    set
        //    {
        //        _mapName = value;
        //        RaisePropertyChangedEvent("MapName");
        //    }
        //}

        //public DateTime? GameDate
        //{
        //    get { return _gameDate; }
        //    set
        //    {
        //        _gameDate = value;
        //        RaisePropertyChangedEvent("GameDate");
        //    }
        //}

        //public TimeSpan GameTime
        //{
        //    get { return _gameTime; }
        //    set
        //    {
        //        _gameTime = value;
        //        RaisePropertyChangedEvent("GameTime");
        //    }
        //}

        //public new ICommand Refresh
        //{
        //    get { return new DelegateCommand(async () => await QueryGameDetails()); }
        //}

        public LastGameViewModel()
            :base()
        {
        }

        protected override Task RefreshExecute()
        {
            throw new NotImplementedException();
        }

        private async Task QueryGameDetails()
        {
            MatchInfoTeam1.Clear();
            MatchInfoTeam2.Clear();

            Replay replay = (await Query.Replay.ReadLastRecordsAsync(1))[0];

            replay = await Query.Replay.ReadReplayIncludeRecord(replay.ReplayId);

            List<ReplayMatchPlayer> players = await Query.MatchPlayer.ReadRecordsByReplayId(replay.ReplayId);
            List<ReplayMatchPlayerTalent> playerTalents = await Query.MatchPlayerTalent.ReadRecordsByReplayId(replay.ReplayId);

            foreach (var player in players)
            {
                MatchInfo matchinfo = new MatchInfo();

                var playerInfo = await Query.HotsPlayer.ReadRecordFromPlayerId(player.PlayerId);
                matchinfo.PlayerName = Utilities.GetNameFromBattleTagName(playerInfo.BattleTagName);
                matchinfo.BattleNetTag = Utilities.GetTagFromBattleTagName(playerInfo.BattleTagName);
                matchinfo.BattleNetId = playerInfo.BattleNetId;
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

            ReplayId = replay.ReplayId;
            GameMode = replay.GameMode;
            MapName = replay.MapName;
            GameDate = replay.TimeStamp;
            GameTime = replay.ReplayLength;
        }
    }
}
