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
