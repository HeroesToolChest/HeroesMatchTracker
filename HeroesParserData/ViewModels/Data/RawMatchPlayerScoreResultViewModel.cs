using HeroesParserData.DataQueries;
using HeroesParserData.Models.DbModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Data
{
    public class RawMatchPlayerScoreResultViewModel : RawDataContext
    {
        private ObservableCollection<ReplayMatchPlayerScoreResult> _replayMatchPlayerScoreResult = new ObservableCollection<ReplayMatchPlayerScoreResult>();

        public ObservableCollection<ReplayMatchPlayerScoreResult> ReplayMatchPlayerScoreResult
        {
            get
            {
                return _replayMatchPlayerScoreResult;
            }
            set
            {
                _replayMatchPlayerScoreResult = value;
                RaisePropertyChangedEvent(nameof(ReplayMatchPlayerScoreResult));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RawMatchPlayerScoreResultViewModel()
            : base()
        {
            AddListColumnNames(new ReplayMatchPlayerScoreResult());
        }

        protected override void ReadDataTop()
        {
            ReplayMatchPlayerScoreResult = new ObservableCollection<ReplayMatchPlayerScoreResult>(Query.MatchPlayerScoreResult.ReadTopRecords(100));
            RowsReturned = ReplayMatchPlayerScoreResult.Count;
        }

        protected override void ReadDataLast()
        {
            ReplayMatchPlayerScoreResult = new ObservableCollection<ReplayMatchPlayerScoreResult>(Query.MatchPlayerScoreResult.ReadLastRecords(100));
            RowsReturned = ReplayMatchPlayerScoreResult.Count;
        }

        protected override void ReadDataCustomTop()
        {
            ReplayMatchPlayerScoreResult = new ObservableCollection<ReplayMatchPlayerScoreResult>(Query.MatchPlayerScoreResult.ReadRecordsCustomTop(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayMatchPlayerScoreResult.Count;
        }

        protected override void ReadDataWhere()
        {
            ReplayMatchPlayerScoreResult = new ObservableCollection<ReplayMatchPlayerScoreResult>(Query.MatchPlayerScoreResult.ReadRecordsWhere(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayMatchPlayerScoreResult.Count;

        }
    }
}
