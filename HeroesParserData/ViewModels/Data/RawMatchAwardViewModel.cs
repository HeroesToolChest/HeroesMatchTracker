using HeroesParserData.DataQueries;
using HeroesParserData.Models.DbModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Data
{
    public class RawMatchAwardViewModel : RawDataContext
    {
        private ObservableCollection<ReplayMatchAward> _replayMatchAward = new ObservableCollection<ReplayMatchAward>();

        public ObservableCollection<ReplayMatchAward> ReplayMatchAward
        {
            get
            {
                return _replayMatchAward;
            }
            set
            {
                _replayMatchAward = value;
                RaisePropertyChangedEvent(nameof(ReplayMatchAward));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RawMatchAwardViewModel()
            : base()
        {
            AddListColumnNames(new ReplayMatchAward());
        }

        protected override void ReadDataTop()
        {
            ReplayMatchAward = new ObservableCollection<ReplayMatchAward>(Query.MatchAward.ReadTopRecords(100));
            RowsReturned = ReplayMatchAward.Count;
        }

        protected override void ReadDataLast()
        {
            ReplayMatchAward = new ObservableCollection<ReplayMatchAward>(Query.MatchAward.ReadLastRecords(100));
            RowsReturned = ReplayMatchAward.Count;
        }

        protected override void ReadDataCustomTop()
        {
            ReplayMatchAward = new ObservableCollection<ReplayMatchAward>(Query.MatchAward.ReadRecordsCustomTop(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayMatchAward.Count;
        }

        protected override void ReadDataWhere()
        {
            ReplayMatchAward = new ObservableCollection<ReplayMatchAward>(Query.MatchAward.ReadRecordsWhere(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayMatchAward.Count;
        }
    }
}
