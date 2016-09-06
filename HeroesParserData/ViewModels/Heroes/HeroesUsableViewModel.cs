using HeroesIcons;
using HeroesParserData.DataQueries.ReplayData;
using HeroesParserData.Models.HeroesModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HeroesParserData.ViewModels.Heroes
{
    public class HeroesUsableViewModel : ViewModelBase
    {
        private HeroesInfo HeroesInfo;
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
            get { return new DelegateCommand(async () => await LoadPlayerHeroes()); }
        }

        public ICommand SelectBattleTagName
        {
            get { return new DelegateCommand(async () => await FindPlayerFromBattleTagName()); }
        }

        public HeroesUsableViewModel()
            :base()
        {
            HeroesInfo = App.HeroesInfo;
            LoadHeroPortraits();
        }

        // loads all hero portraits
        private void LoadHeroPortraits()
        {
            var listOfHeroes = HeroesInfo.GetListOfHeroes();

            int i = 0;
            while (i < listOfHeroes.Count)
            {
                HeroesUsable heroesUsable = new HeroesUsable();

                heroesUsable.HeroName1 = listOfHeroes[i];
                heroesUsable.HeroPortrait1 = HeroesInfo.GetHeroPortrait(heroesUsable.HeroName1);
                i++;
                if (i >= listOfHeroes.Count) { Heroes.Add(heroesUsable); break; }

                heroesUsable.HeroName2 = listOfHeroes[i];
                heroesUsable.HeroPortrait2 = HeroesInfo.GetHeroPortrait(heroesUsable.HeroName2);
                i++;
                if (i >= listOfHeroes.Count) { Heroes.Add(heroesUsable); break; }

                heroesUsable.HeroName3 = listOfHeroes[i];
                heroesUsable.HeroPortrait3 = HeroesInfo.GetHeroPortrait(heroesUsable.HeroName3);
                i++;
                if (i >= listOfHeroes.Count) { Heroes.Add(heroesUsable); break; }

                heroesUsable.HeroName4 = listOfHeroes[i];
                heroesUsable.HeroPortrait4 = HeroesInfo.GetHeroPortrait(heroesUsable.HeroName4);
                i++;
                if (i >= listOfHeroes.Count) { Heroes.Add(heroesUsable); break; }

                heroesUsable.HeroName5 = listOfHeroes[i];
                heroesUsable.HeroPortrait5 = HeroesInfo.GetHeroPortrait(heroesUsable.HeroName5);
                i++;
                if (i >= listOfHeroes.Count) { Heroes.Add(heroesUsable); break; }

                heroesUsable.HeroName6 = listOfHeroes[i];
                heroesUsable.HeroPortrait6 = HeroesInfo.GetHeroPortrait(heroesUsable.HeroName6);
                i++;
                if (i >= listOfHeroes.Count) { Heroes.Add(heroesUsable); break; }

                heroesUsable.HeroName7 = listOfHeroes[i];
                heroesUsable.HeroPortrait7 = HeroesInfo.GetHeroPortrait(heroesUsable.HeroName7);
                i++;
                if (i >= listOfHeroes.Count) { Heroes.Add(heroesUsable); break; }

                heroesUsable.HeroName8 = listOfHeroes[i];
                heroesUsable.HeroPortrait8 = HeroesInfo.GetHeroPortrait(heroesUsable.HeroName8);
                i++;
                if (i >= listOfHeroes.Count) { Heroes.Add(heroesUsable); break; }

                heroesUsable.HeroName9 = listOfHeroes[i];
                heroesUsable.HeroPortrait9 = HeroesInfo.GetHeroPortrait(heroesUsable.HeroName9);
                i++;
                if (i >= listOfHeroes.Count) { Heroes.Add(heroesUsable); break; }

                heroesUsable.HeroName10 = listOfHeroes[i];
                heroesUsable.HeroPortrait10 = HeroesInfo.GetHeroPortrait(heroesUsable.HeroName10);
                i++;
                if (i >= listOfHeroes.Count) { Heroes.Add(heroesUsable); break; }

                heroesUsable.HeroName11 = listOfHeroes[i];
                heroesUsable.HeroPortrait11 = HeroesInfo.GetHeroPortrait(heroesUsable.HeroName11);
                i++;
                if (i >= listOfHeroes.Count) { Heroes.Add(heroesUsable); break; }

                heroesUsable.HeroName12 = listOfHeroes[i];
                heroesUsable.HeroPortrait12 = HeroesInfo.GetHeroPortrait(heroesUsable.HeroName12);
                i++;
                if (i >= listOfHeroes.Count) { Heroes.Add(heroesUsable); break; }

                heroesUsable.HeroName13 = listOfHeroes[i];
                heroesUsable.HeroPortrait13 = HeroesInfo.GetHeroPortrait(heroesUsable.HeroName13);
                i++;
                if (i >= listOfHeroes.Count) { Heroes.Add(heroesUsable); break; }

                heroesUsable.HeroName14 = listOfHeroes[i];
                heroesUsable.HeroPortrait14 = HeroesInfo.GetHeroPortrait(heroesUsable.HeroName14);
                i++;
                if (i >= listOfHeroes.Count) { Heroes.Add(heroesUsable); break; }

                heroesUsable.HeroName15 = listOfHeroes[i];
                heroesUsable.HeroPortrait15 = HeroesInfo.GetHeroPortrait(heroesUsable.HeroName15);
                i++;
                if (i >= listOfHeroes.Count) { Heroes.Add(heroesUsable); break; }
                Heroes.Add(heroesUsable);
            }
        }

        private async Task LoadPlayerHeroes()
        {
            await LoadPlayerHeroesUsable(TextBoxPlayerId);
        }

        private async Task LoadPlayerHeroesUsable(long playerId)
        {
            // may not get all heroes depending on last seen
            var player = await Query.HotsPlayer.ReadRecordFromPlayerId(playerId);

            if (player == null)
                return;

            var heroRecords = await Query.HotsPlayerHero.ReadListOfHeroRecordsForPlayerIdAsync(playerId);

            if (heroRecords.Count == 0)
            {
                int count = 0;
                int totalHeroes = HeroesInfo.TotalAmountOfHeroes();
                for (int i = 0; i < Heroes.Count; i++)
                {
                    Heroes[i].IsXOut1 = true; count++; if (count >= totalHeroes) break;
                    Heroes[i].IsXOut2 = true; count++; if (count >= totalHeroes) break;
                    Heroes[i].IsXOut3 = true; count++; if (count >= totalHeroes) break;
                    Heroes[i].IsXOut4 = true; count++; if (count >= totalHeroes) break;
                    Heroes[i].IsXOut5 = true; count++; if (count >= totalHeroes) break;
                    Heroes[i].IsXOut6 = true; count++; if (count >= totalHeroes) break;
                    Heroes[i].IsXOut7 = true; count++; if (count >= totalHeroes) break;
                    Heroes[i].IsXOut8 = true; count++; if (count >= totalHeroes) break;
                    Heroes[i].IsXOut9 = true; count++; if (count >= totalHeroes) break;
                    Heroes[i].IsXOut10 = true; count++; if (count >= totalHeroes) break;
                    Heroes[i].IsXOut11 = true; count++; if (count >= totalHeroes) break;
                    Heroes[i].IsXOut12 = true; count++; if (count >= totalHeroes) break;
                    Heroes[i].IsXOut13 = true; count++; if (count >= totalHeroes) break;
                    Heroes[i].IsXOut14 = true; count++; if (count >= totalHeroes) break;
                    Heroes[i].IsXOut15 = true; count++; if (count >= totalHeroes) break;
                }

                PlayerInfo = $"{player.BattleTagName}  Last Seen: {player.LastSeen}  -- NO HERO INFO FOUND --";
            }
            else
            {
                foreach (var heroRecord in heroRecords)
                {
                    for (int i = 0; i < Heroes.Count; i++)
                    {
                        if (Heroes[i].HeroName1 == heroRecord.HeroName)
                        {
                            Heroes[i].IsXOut1 = !heroRecord.IsUsable;
                            break;
                        }
                        else if (Heroes[i].HeroName2 == heroRecord.HeroName)
                        {
                            Heroes[i].IsXOut2 = !heroRecord.IsUsable;
                            break;
                        }
                        else if (Heroes[i].HeroName3 == heroRecord.HeroName)
                        {
                            Heroes[i].IsXOut3 = !heroRecord.IsUsable;
                            break;
                        }
                        else if (Heroes[i].HeroName4 == heroRecord.HeroName)
                        {
                            Heroes[i].IsXOut4 = !heroRecord.IsUsable;
                            break;
                        }
                        else if (Heroes[i].HeroName5 == heroRecord.HeroName)
                        {
                            Heroes[i].IsXOut5 = !heroRecord.IsUsable;
                            break;
                        }
                        else if (Heroes[i].HeroName6 == heroRecord.HeroName)
                        {
                            Heroes[i].IsXOut6 = !heroRecord.IsUsable;
                            break;
                        }
                        else if (Heroes[i].HeroName7 == heroRecord.HeroName)
                        {
                            Heroes[i].IsXOut7 = !heroRecord.IsUsable;
                            break;
                        }
                        else if (Heroes[i].HeroName8 == heroRecord.HeroName)
                        {
                            Heroes[i].IsXOut8 = !heroRecord.IsUsable;
                            break;
                        }
                        else if (Heroes[i].HeroName9 == heroRecord.HeroName)
                        {
                            Heroes[i].IsXOut9 = !heroRecord.IsUsable;
                            break;
                        }
                        else if (Heroes[i].HeroName10 == heroRecord.HeroName)
                        {
                            Heroes[i].IsXOut10 = !heroRecord.IsUsable;
                            break;
                        }
                        else if (Heroes[i].HeroName10 == heroRecord.HeroName)
                        {
                            Heroes[i].IsXOut10 = !heroRecord.IsUsable;
                            break;
                        }
                        else if (Heroes[i].HeroName11 == heroRecord.HeroName)
                        {
                            Heroes[i].IsXOut11 = !heroRecord.IsUsable;
                            break;
                        }
                        else if (Heroes[i].HeroName12 == heroRecord.HeroName)
                        {
                            Heroes[i].IsXOut12 = !heroRecord.IsUsable;
                            break;
                        }
                        else if (Heroes[i].HeroName13 == heroRecord.HeroName)
                        {
                            Heroes[i].IsXOut13 = !heroRecord.IsUsable;
                            break;
                        }
                        else if (Heroes[i].HeroName14 == heroRecord.HeroName)
                        {
                            Heroes[i].IsXOut14 = !heroRecord.IsUsable;
                            break;
                        }
                        else if (Heroes[i].HeroName15 == heroRecord.HeroName)
                        {
                            Heroes[i].IsXOut15 = !heroRecord.IsUsable;
                            break;
                        }
                    }
                }

                PlayerInfo = $"{player.BattleTagName}  Last Seen: {player.LastSeen}";
            }
        }

        private async Task FindPlayerFromBattleTagName()
        {
            var records = await Query.HotsPlayer.ReadRecordsWhereAsync("BattleTagName", "LIKE", $"%{TextBoxBattleTagName.Trim()}%");

            if (records.Count == 1)
            {
                await LoadPlayerHeroesUsable(records[0].PlayerId);
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
