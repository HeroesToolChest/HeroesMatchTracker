using HeroesParserData.DataQueries.ReplayData;
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
            AddListColumnNames();
        }

        private void AddListColumnNames()
        {
            ReplayMatchTeamLevel r = new ReplayMatchTeamLevel();

            foreach (var prop in r.GetType().GetMethods())
            {
                if (prop.IsVirtual == false && prop.ReturnType.Name == "Void")
                {
                    string columnName = prop.Name.Split('_')[1];
                    if (!columnName.Contains("Ticks"))
                        ColumnNames.Add(columnName);
                }
            }
        }

        protected override async Task ReadDataTop()
        {
            ReplayMatchTeamLevel = new ObservableCollection<ReplayMatchTeamLevel>(await Query.MatchTeamLevel.ReadTopRecordsAsync(100));
            RowsReturned = ReplayMatchTeamLevel.Count;
        }

        protected override async Task ReadDataLast()
        {
            ReplayMatchTeamLevel = new ObservableCollection<ReplayMatchTeamLevel>(await Query.MatchTeamLevel.ReadLastRecordsAsync(100));
            RowsReturned = ReplayMatchTeamLevel.Count;
        }

        protected override async Task ReadDataCustomTop()
        {
            ReplayMatchTeamLevel = new ObservableCollection<ReplayMatchTeamLevel>(await Query.MatchTeamLevel.ReadRecordsCustomTopAsync(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayMatchTeamLevel.Count;
        }

        protected override async Task ReadDataWhere()
        {
            ReplayMatchTeamLevel = new ObservableCollection<ReplayMatchTeamLevel>(await Query.MatchTeamLevel.ReadRecordsWhereAsync(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayMatchTeamLevel.Count;
        }
    }
}
