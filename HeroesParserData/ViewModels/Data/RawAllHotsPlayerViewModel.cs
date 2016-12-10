using HeroesParserData.DataQueries;
using HeroesParserData.Models.DbModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Data
{
    public class RawAllHotsPlayerViewModel : RawDataContext
    {
        private ObservableCollection<ReplayAllHotsPlayer> _replayAllHotsPlayer = new ObservableCollection<ReplayAllHotsPlayer>();

        public ObservableCollection<ReplayAllHotsPlayer> ReplayAllHotsPlayer
        {
            get
            {
                return _replayAllHotsPlayer;
            }
            set
            {
                _replayAllHotsPlayer = value;
                RaisePropertyChangedEvent(nameof(ReplayAllHotsPlayer));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RawAllHotsPlayerViewModel()
            : base()
        {
            AddListColumnNames(new ReplayAllHotsPlayer());
        }

        protected override void ReadDataTop()
        {
            ReplayAllHotsPlayer = new ObservableCollection<ReplayAllHotsPlayer>(Query.HotsPlayer.ReadTopRecords(100));
            RowsReturned = ReplayAllHotsPlayer.Count;
        }

        protected override void ReadDataLast()
        {
            ReplayAllHotsPlayer = new ObservableCollection<ReplayAllHotsPlayer>(Query.HotsPlayer.ReadLastRecords(100));
            RowsReturned = ReplayAllHotsPlayer.Count;
        }

        protected override void ReadDataCustomTop()
        {
            ReplayAllHotsPlayer = new ObservableCollection<ReplayAllHotsPlayer>(Query.HotsPlayer.ReadRecordsCustomTop(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayAllHotsPlayer.Count;
        }

        protected override void ReadDataWhere()
        {
            ReplayAllHotsPlayer = new ObservableCollection<ReplayAllHotsPlayer>(Query.HotsPlayer.ReadRecordsWhere(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayAllHotsPlayer.Count;
        }
    }
}
