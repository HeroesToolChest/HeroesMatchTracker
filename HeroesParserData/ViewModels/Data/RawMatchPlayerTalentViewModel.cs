using HeroesParserData.DataQueries;
using HeroesParserData.Models.DbModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Data
{
    public class RawMatchPlayerTalentViewModel : RawDataContext
    {
        private ObservableCollection<ReplayMatchPlayerTalent> _replayMatchPlayerTalent = new ObservableCollection<ReplayMatchPlayerTalent>();

        public ObservableCollection<ReplayMatchPlayerTalent> ReplayMatchPlayerTalent
        {
            get
            {
                return _replayMatchPlayerTalent;
            }
            set
            {
                _replayMatchPlayerTalent = value;
                RaisePropertyChangedEvent(nameof(ReplayMatchPlayerTalent));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RawMatchPlayerTalentViewModel()
            : base()
        {
            AddListColumnNames(new ReplayMatchPlayerTalent());
        }

        protected override void ReadDataTop()
        {
            ReplayMatchPlayerTalent = new ObservableCollection<ReplayMatchPlayerTalent>(Query.MatchPlayerTalent.ReadTopRecords(100));
            RowsReturned = ReplayMatchPlayerTalent.Count;

        }

        protected override void ReadDataLast()
        {
            ReplayMatchPlayerTalent = new ObservableCollection<ReplayMatchPlayerTalent>(Query.MatchPlayerTalent.ReadLastRecords(100));
            RowsReturned = ReplayMatchPlayerTalent.Count;

        }

        protected override void ReadDataCustomTop()
        {
            ReplayMatchPlayerTalent = new ObservableCollection<ReplayMatchPlayerTalent>(Query.MatchPlayerTalent.ReadRecordsCustomTop(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayMatchPlayerTalent.Count;

        }

        protected override void ReadDataWhere()
        {
            ReplayMatchPlayerTalent = new ObservableCollection<ReplayMatchPlayerTalent>(Query.MatchPlayerTalent.ReadRecordsWhere(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayMatchPlayerTalent.Count;
        }
    }
}
