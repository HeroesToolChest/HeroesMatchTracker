using HeroesParserData.DataQueries.ReplayData;
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
            AddListColumnNames();
        }

        private void AddListColumnNames()
        {
            ReplayMatchPlayerScoreResult r = new ReplayMatchPlayerScoreResult();

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
            ReplayMatchPlayerScoreResult = new ObservableCollection<ReplayMatchPlayerScoreResult>(await Query.MatchPlayerScoreResult.ReadTopRecordsAsync(100));
            RowsReturned = ReplayMatchPlayerScoreResult.Count;
        }

        protected override async Task ReadDataLast()
        {
            ReplayMatchPlayerScoreResult = new ObservableCollection<ReplayMatchPlayerScoreResult>(await Query.MatchPlayerScoreResult.ReadLastRecordsAsync(100));
            RowsReturned = ReplayMatchPlayerScoreResult.Count;
        }

        protected override async Task ReadDataCustomTop()
        {
            ReplayMatchPlayerScoreResult = new ObservableCollection<ReplayMatchPlayerScoreResult>(await Query.MatchPlayerScoreResult.ReadRecordsCustomTopAsync(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayMatchPlayerScoreResult.Count;
        }

        protected override async Task ReadDataWhere()
        {
            ReplayMatchPlayerScoreResult = new ObservableCollection<ReplayMatchPlayerScoreResult>(await Query.MatchPlayerScoreResult.ReadRecordsWhereAsync(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayMatchPlayerScoreResult.Count;

        }
    }
}
