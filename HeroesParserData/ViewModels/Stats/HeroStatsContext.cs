using GalaSoft.MvvmLight.Messaging;
using HeroesParserData.DataQueries;
using HeroesParserData.Messages;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HeroesParserData.ViewModels.Stats
{
    public abstract class HeroStatsContext : ViewModelBase
    {
        private string _selectedSeasonOption;
        private bool _gameModeListVisibility;
        private bool _isComboBoxEnabled;

        private List<string> _seasonList = new List<string>();

        protected Season GetSeasonSelected
        {
            get { return Utilities.GetSeasonFromString(SelectedSeasonOption); }
        }

        public List<string> SeasonList
        {
            get { return _seasonList; }
            set
            {
                _seasonList = value;
                RaisePropertyChangedEvent(nameof(SeasonList));
            }
        }

        public string SelectedSeasonOption
        {
            get { return _selectedSeasonOption; }
            set
            {
                _selectedSeasonOption = value;
                RaisePropertyChangedEvent(nameof(SelectedSeasonOption));

            }
        }

        public bool GameModeListVisibility
        {
            get { return _gameModeListVisibility; }
            set
            {
                _gameModeListVisibility = value;
                RaisePropertyChangedEvent(nameof(GameModeListVisibility));
            }
        }

        public bool IsComboBoxEnabled
        {
            get { return _isComboBoxEnabled; }
            set
            {
                _isComboBoxEnabled = value;
                RaisePropertyChangedEvent(nameof(IsComboBoxEnabled));
            }
        }

        public ICommand RefreshStatsCommand
        {
            get { return new DelegateCommand(async () => await PerformCommand()); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HeroStatsContext()
            :base()
        {
            Messenger.Default.Register<StatisticsTabMessage>(this, async (action) => await ReceiveMessage(action));
            InitializeLists();
            IsComboBoxEnabled = true;
        }

        protected abstract Task RefreshStats();
        protected abstract Task ReceiveMessage(StatisticsTabMessage action);

        protected int QueryHeroLevel(string heroName)
        {
            return Query.HeroStatsGameMode.GetHighestLevelOfHero(heroName);
        }

        protected async Task PerformCommand()
        {
            IsComboBoxEnabled = false;
            await Task.Run(async () =>
            {
                try
                {
                    await RefreshStats();
                }
                catch (Exception ex)
                {
                    ExceptionLog.Log(LogLevel.Error, ex);
                }
            });
            IsComboBoxEnabled = true;
        }

        private void InitializeLists()
        {
            SeasonList.Add("Lifetime");
            SeasonList.AddRange(Utilities.GetSeasonList());

            SelectedSeasonOption = SeasonList[SeasonList.Count - 1];
        }
    }
}
