using GalaSoft.MvvmLight.Messaging;
using HeroesParserData.DataQueries;
using HeroesParserData.Messages;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HeroesParserData.ViewModels.Stats.HeroStats
{
    public abstract class HeroStatsContext : ViewModelBase
    {
        private string _selectedSeasonOption;

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

        public ICommand RefreshStatsCommand
        {
            get { return new DelegateCommand(() => PerformCommand()); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HeroStatsContext()
            :base()
        {
            Messenger.Default.Register<StatisticsTabMessage>(this, (action) => ReceiveMessage(action));
            InitializeLists();
        }

        protected abstract Task RefreshStats();
        protected abstract void ReceiveMessage(StatisticsTabMessage action);

        protected int QueryHeroLevels(string heroName)
        {
            return Query.HeroStatsGameMode.GetHighestLevelOfHero(heroName);
        }

        protected void PerformCommand()
        {
            Task.Run(async () =>
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
        }

        private void InitializeLists()
        {
            SeasonList.Add("Lifetime");
            SeasonList.AddRange(Utilities.GetSeasonList());

            SelectedSeasonOption = SeasonList[SeasonList.Count - 1];
        }
    }
}
