using HeroesParserData.DataQueries;
using HeroesParserData.Models.DbModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Data
{
    public class RawMatchTeamLevelViewModel : RawDataContext
    {
        private ObservableCollection<ReplayMatchTeamLevel> _replayMatchTeamLevel = new ObservableCollection<ReplayMatchTeamLevel>();

        public ObservableCollection<ReplayMatchTeamLevel> ReplayMatchTeamLevel
        {
            get
            {
                return _replayMatchTeamLevel;
            }
            set
            {
                _replayMatchTeamLevel = value;
                RaisePropertyChangedEvent(nameof(ReplayMatchTeamLevel));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RawMatchTeamLevelViewModel()
            : base()
        {
            AddListColumnNames(new ReplayMatchTeamLevel());
        }

        protected override void ReadDataTop()
        {
            ReplayMatchTeamLevel = new ObservableCollection<ReplayMatchTeamLevel>(Query.MatchTeamLevel.ReadTopRecords(100));
            RowsReturned = ReplayMatchTeamLevel.Count;
        }

        protected override void ReadDataLast()
        {
            ReplayMatchTeamLevel = new ObservableCollection<ReplayMatchTeamLevel>(Query.MatchTeamLevel.ReadLastRecords(100));
            RowsReturned = ReplayMatchTeamLevel.Count;
        }

        protected override void ReadDataCustomTop()
        {
            ReplayMatchTeamLevel = new ObservableCollection<ReplayMatchTeamLevel>(Query.MatchTeamLevel.ReadRecordsCustomTop(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayMatchTeamLevel.Count;
        }

        protected override void ReadDataWhere()
        {
            ReplayMatchTeamLevel = new ObservableCollection<ReplayMatchTeamLevel>(Query.MatchTeamLevel.ReadRecordsWhere(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayMatchTeamLevel.Count;
        }
    }
}
