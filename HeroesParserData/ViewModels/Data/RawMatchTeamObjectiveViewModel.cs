using HeroesParserData.DataQueries;
using HeroesParserData.Models.DbModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Data
{
    public class RawMatchTeamObjectiveViewModel : RawDataContext
    {
        private ObservableCollection<ReplayMatchTeamObjective> _replayMatchTeamObjective = new ObservableCollection<ReplayMatchTeamObjective>();

        public ObservableCollection<ReplayMatchTeamObjective> ReplayMatchTeamObjective
        {
            get
            {
                return _replayMatchTeamObjective;
            }
            set
            {
                _replayMatchTeamObjective = value;
                RaisePropertyChangedEvent(nameof(ReplayMatchTeamObjective));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RawMatchTeamObjectiveViewModel()
            : base()
        {
            AddListColumnNames(new ReplayMatchTeamObjective());
        }

        protected override void ReadDataTop()
        {
            ReplayMatchTeamObjective = new ObservableCollection<ReplayMatchTeamObjective>(Query.MatchTeamObjective.ReadTopRecords(100));
            RowsReturned = ReplayMatchTeamObjective.Count;
        }

        protected override void ReadDataLast()
        {
            ReplayMatchTeamObjective = new ObservableCollection<ReplayMatchTeamObjective>(Query.MatchTeamObjective.ReadLastRecords(100));
            RowsReturned = ReplayMatchTeamObjective.Count;
        }

        protected override void ReadDataCustomTop()
        {
            ReplayMatchTeamObjective = new ObservableCollection<ReplayMatchTeamObjective>(Query.MatchTeamObjective.ReadRecordsCustomTop(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayMatchTeamObjective.Count;
        }

        protected override void ReadDataWhere()
        {
            ReplayMatchTeamObjective = new ObservableCollection<ReplayMatchTeamObjective>(Query.MatchTeamObjective.ReadRecordsWhere(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayMatchTeamObjective.Count;
        }
    }
}
