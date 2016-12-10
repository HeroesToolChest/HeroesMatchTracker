using HeroesParserData.DataQueries;
using HeroesParserData.Models.DbModels;
using System.Collections.ObjectModel;

namespace HeroesParserData.ViewModels.Data
{
    public class RawReplayViewModel : RawDataContext
    {
        private ObservableCollection<Replay> _replays = new ObservableCollection<Replay>();

        public ObservableCollection<Replay> Replays
        {
            get
            {
                return _replays;
            }
            set
            {
                _replays = value;
                RaisePropertyChangedEvent(nameof(Replays));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RawReplayViewModel()
            : base()
        {
            AddListColumnNames(new Replay());
        }

        protected override void ReadDataTop()
        {
            Replays = new ObservableCollection<Replay>(Query.Replay.ReadTopRecords(100));
            RowsReturned = Replays.Count;
        }

        protected override void ReadDataLast()
        {
            Replays = new ObservableCollection<Replay>(Query.Replay.ReadLastRecords(100));
            RowsReturned = Replays.Count;
        }

        protected override void ReadDataCustomTop()
        {
            Replays = new ObservableCollection<Replay>(Query.Replay.ReadRecordsCustomTop(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = Replays.Count;
        }

        protected override void ReadDataWhere()
        {
            Replays = new ObservableCollection<Replay>(Query.Replay.ReadRecordsWhere(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = Replays.Count;
        }
    }
}
