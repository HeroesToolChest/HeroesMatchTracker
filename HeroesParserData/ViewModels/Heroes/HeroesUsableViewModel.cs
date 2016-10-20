using HeroesIcons;
using HeroesParserData.DataQueries;
using HeroesParserData.Models.HeroesModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HeroesParserData.ViewModels.Heroes
{
    public class HeroesUsableViewModel : ViewModelBase
    {
        private Dictionary<string, Tuple<int, int>> HeroesGridLocation = new Dictionary<string, Tuple<int, int>>();
        private ObservableCollection<HeroesUsable> _heroes = new ObservableCollection<HeroesUsable>();

        private long _textBoxPlayerId;
        private string _textBoxBattleTagName;
        private string _battleTagNameResult;
        private string _playerInfo;

        public long TextBoxPlayerId
        {
            get { return _textBoxPlayerId; }
            set
            {
                _textBoxPlayerId = value;
                RaisePropertyChangedEvent(nameof(TextBoxPlayerId));
            }
        }

        public string TextBoxBattleTagName
        {
            get { return _textBoxBattleTagName; }
            set
            {
                _textBoxBattleTagName = value;
                RaisePropertyChangedEvent(nameof(TextBoxBattleTagName));
            }
        }

        public string BattleTagNameResult
        {
            get { return _battleTagNameResult; }
            set
            {
                _battleTagNameResult = value;
                RaisePropertyChangedEvent(nameof(BattleTagNameResult));
            }
        }

        public string PlayerInfo
        {
            get { return _playerInfo; }
            set
            {
                _playerInfo = value;
                RaisePropertyChangedEvent(nameof(PlayerInfo));
            }
        }

        public ObservableCollection<HeroesUsable> Heroes
        {
            get { return _heroes; }
            set
            {
                _heroes = value;
                RaisePropertyChangedEvent(nameof(Heroes));
            }
        }

        public ICommand SelectPlayerId
        {
            get { return new DelegateCommand(() => LoadPlayerHeroes()); }
        }

        public ICommand SelectBattleTagName
        {
            get { return new DelegateCommand(() => FindPlayerFromBattleTagName()); }
        }

        public HeroesUsableViewModel()
            :base()
        {
            LoadHeroPortraits();
        }

        // loads all hero portraits
        private void LoadHeroPortraits()
        {
            var listOfHeroes = HeroesInfo.GetListOfHeroes();
            int heroCount = 0;
            int row = 0;

            while (heroCount < listOfHeroes.Count)
            {
                HeroesUsable heroesUsable = new HeroesUsable();

                for (int column = 0; column < 15; column++)
                {
                    if (heroCount < listOfHeroes.Count)
                    {
                        heroesUsable.HeroName[column] = listOfHeroes[heroCount];
                        heroesUsable.HeroPortrait[column] = HeroesInfo.GetHeroPortrait(listOfHeroes[heroCount]);
                        heroesUsable.IsXOut.Add(true);
                        HeroesGridLocation.Add(listOfHeroes[heroCount], new Tuple<int, int>(row, column));
                    }
                    else
                        heroesUsable.IsXOut.Add(false);

                    heroCount++;
                }

                Heroes.Add(heroesUsable);
                row++;
            }
        }

        private void LoadPlayerHeroes()
        {
            LoadPlayerHeroesUsable(TextBoxPlayerId);
        }

        private void LoadPlayerHeroesUsable(long playerId)
        {
            // may not get all heroes depending on last seen
            var player = Query.HotsPlayer.ReadRecordFromPlayerId(playerId);

            if (player == null)
                return;

            var heroRecords = Query.HotsPlayerHero.ReadListOfHeroRecordsForPlayerIdAsync(playerId);

            int heroesUsable = 0;

            if (heroRecords.Count == 0)
            {
                int count = 0;
                foreach (var row in Heroes)
                {
                    for (int i = 0; i < row.IsXOut.Count; i++)
                    {
                        if (count >= Heroes.Count)
                            break;

                        row.IsXOut[i] = true;
                        count++;
                        
                    }
                }

                PlayerInfo = $"{player.BattleTagName}  Last Seen: {player.LastSeen}  -- NO HERO INFO FOUND --";
            }
            else
            {
                foreach (var hero in heroRecords)
                { 
                    string realHeroName = HeroesInfo.GetRealHeroNameFromAltName(hero.HeroName);

                    if (HeroesGridLocation.ContainsKey(realHeroName))
                    {
                        int row = HeroesGridLocation[realHeroName].Item1;
                        int column = HeroesGridLocation[realHeroName].Item2;

                        if (Heroes[row].HeroName[column] == realHeroName)
                        {
                            Heroes[row].IsXOut[column] = !hero.IsUsable;
                            if (hero.IsUsable)
                                heroesUsable++;
                        }
                        else
                            ExceptionLog.Log(LogLevel.Info, $"{Heroes[row].HeroName[column]} != {realHeroName}");
                    }
                    else
                        ExceptionLog.Log(LogLevel.Info, $"HeroesGridLocation does not contain {realHeroName}");
                }


                PlayerInfo = $"{player.BattleTagName}  Last Seen: {player.LastSeen} [{heroesUsable} out of {HeroesInfo.TotalAmountOfHeroes()}]";
            }
        }

        private void FindPlayerFromBattleTagName()
        {
            if (string.IsNullOrEmpty(TextBoxBattleTagName))
                return;

            var records = Query.HotsPlayer.ReadRecordsWhere("BattleTagName", "LIKE", $"%{TextBoxBattleTagName.Trim()}%");

            if (records.Count == 1)
            {
                LoadPlayerHeroesUsable(records[0].PlayerId);
                BattleTagNameResult = $"Player found: {records[0].BattleTagName}";
            }
            else if (records.Count <= 0)
            {
                BattleTagNameResult = "No players found";
            }
            else if (records.Count > 1)
            {
                BattleTagNameResult = $"{records.Count} results found, top 5: ";

                for (int i = 0; i < records.Count; i++)
                {
                    BattleTagNameResult += records[i].BattleTagName;

                    if (i == 4 || i == records.Count - 1)
                        break;

                    BattleTagNameResult += ", ";
                }
            }
        }
    }
}
