using HeroesParserData.DataQueries;
using HeroesParserData.Models.DbModels;
using System.Collections.ObjectModel;

namespace HeroesParserData.ViewModels.Data
{
    public class RawAllHotsPlayerHeroViewModel : RawDataContext
    {
        private ObservableCollection<ReplayAllHotsPlayerHero> _replayAllHotsPlayerHero = new ObservableCollection<ReplayAllHotsPlayerHero>();

        public ObservableCollection<ReplayAllHotsPlayerHero> ReplayAllHotsPlayerHero
        {
            get
            {
                return _replayAllHotsPlayerHero;
            }
            set
            {
                _replayAllHotsPlayerHero = value;
                RaisePropertyChangedEvent(nameof(ReplayAllHotsPlayerHero));
            }
        }

        public RawAllHotsPlayerHeroViewModel()
            : base()
        {
            AddListColumnNames(new ReplayAllHotsPlayerHero());
        }

        protected override void ReadDataTop()
        {
            ReplayAllHotsPlayerHero = new ObservableCollection<ReplayAllHotsPlayerHero>(Query.HotsPlayerHero.ReadTopRecords(100));
            RowsReturned = ReplayAllHotsPlayerHero.Count;
        }

        protected override void ReadDataLast()
        {
            ReplayAllHotsPlayerHero = new ObservableCollection<ReplayAllHotsPlayerHero>(Query.HotsPlayerHero.ReadLastRecords(100));
            RowsReturned = ReplayAllHotsPlayerHero.Count;
        }

        protected override void ReadDataCustomTop()
        {
            ReplayAllHotsPlayerHero = new ObservableCollection<ReplayAllHotsPlayerHero>(Query.HotsPlayerHero.ReadRecordsCustomTop(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayAllHotsPlayerHero.Count;
        }

        protected override void ReadDataWhere()
        {
            ReplayAllHotsPlayerHero = new ObservableCollection<ReplayAllHotsPlayerHero>(Query.HotsPlayerHero.ReadRecordsWhere(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayAllHotsPlayerHero.Count;
        }
    }
}
