using HeroesParserData.DataQueries.ReplayData;
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
                RaisePropertyChangedEvent("ReplayMatchTeamBan");
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RawMatchTeamBanViewModel()
            : base()
        {
            AddListColumnNames();
        }

        private void AddListColumnNames()
        {
            ReplayMatchTeamBan r = new ReplayMatchTeamBan();

            foreach (var prop in r.GetType().GetMethods())
            {
                if (prop.IsVirtual == false && prop.ReturnType.Name == "Void")
                    ColumnNames.Add(prop.Name.Split('_')[1]);
            }
        }

        protected override async Task ReadDataTop()
        {
            ReplayMatchTeamBan = new ObservableCollection<ReplayMatchTeamBan>(await Query.MatchTeamBan.ReadTopRecordsAsync(100));
            RowsReturned = ReplayMatchTeamBan.Count;
        }

        protected override async Task ReadDataLast()
        {
            ReplayMatchTeamBan = new ObservableCollection<ReplayMatchTeamBan>(await Query.MatchTeamBan.ReadLastRecordsAsync(100));
            RowsReturned = ReplayMatchTeamBan.Count;
        }

        protected override async Task ReadDataCustomTop()
        {
            ReplayMatchTeamBan = new ObservableCollection<ReplayMatchTeamBan>(await Query.MatchTeamBan.ReadRecordsCustomTopAsync(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayMatchTeamBan.Count;
        }

        protected override async Task ReadDataWhere()
        {
            ReplayMatchTeamBan = new ObservableCollection<ReplayMatchTeamBan>(await Query.MatchTeamBan.ReadRecordsWhereAsync(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayMatchTeamBan.Count;
        }
    }
}
