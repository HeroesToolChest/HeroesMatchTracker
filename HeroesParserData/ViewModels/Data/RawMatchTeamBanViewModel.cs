using HeroesParserData.DataQueries;
using HeroesParserData.Models.DbModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Data
{
    public class RawMatchTeamBanViewModel : RawDataContext
    {
        private ObservableCollection<ReplayMatchTeamBan> _replayMatchTeamBan = new ObservableCollection<ReplayMatchTeamBan>();

        public ObservableCollection<ReplayMatchTeamBan> ReplayMatchTeamBan
        {
            get
            {
                return _replayMatchTeamBan;
            }
            set
            {
                _replayMatchTeamBan = value;
                RaisePropertyChangedEvent(nameof(ReplayMatchTeamBan));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RawMatchTeamBanViewModel()
            : base()
        {
            AddListColumnNames(new ReplayMatchTeamBan());
        }

        protected override void ReadDataTop()
        {
            ReplayMatchTeamBan = new ObservableCollection<ReplayMatchTeamBan>(Query.MatchTeamBan.ReadTopRecords(100));
            RowsReturned = ReplayMatchTeamBan.Count;
        }

        protected override void ReadDataLast()
        {
            ReplayMatchTeamBan = new ObservableCollection<ReplayMatchTeamBan>(Query.MatchTeamBan.ReadLastRecords(100));
            RowsReturned = ReplayMatchTeamBan.Count;
        }

        protected override void ReadDataCustomTop()
        {
            ReplayMatchTeamBan = new ObservableCollection<ReplayMatchTeamBan>(Query.MatchTeamBan.ReadRecordsCustomTop(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayMatchTeamBan.Count;
        }

        protected override void ReadDataWhere()
        {
            ReplayMatchTeamBan = new ObservableCollection<ReplayMatchTeamBan>(Query.MatchTeamBan.ReadRecordsWhere(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayMatchTeamBan.Count;
        }
    }
}
