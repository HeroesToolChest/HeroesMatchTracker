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
            AddListColumnNames();
        }

        private void AddListColumnNames()
        {
            ReplayMatchAward r = new ReplayMatchAward();

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
            ReplayMatchAward = new ObservableCollection<ReplayMatchAward>(await Query.MatchAward.ReadTopRecordsAsync(100));
            RowsReturned = ReplayMatchAward.Count;
        }

        protected override async Task ReadDataLast()
        {
            ReplayMatchAward = new ObservableCollection<ReplayMatchAward>(await Query.MatchAward.ReadLastRecordsAsync(100));
            RowsReturned = ReplayMatchAward.Count;
        }

        protected override async Task ReadDataCustomTop()
        {
            ReplayMatchAward = new ObservableCollection<ReplayMatchAward>(await Query.MatchAward.ReadRecordsCustomTopAsync(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayMatchAward.Count;
        }

        protected override async Task ReadDataWhere()
        {
            ReplayMatchAward = new ObservableCollection<ReplayMatchAward>(await Query.MatchAward.ReadRecordsWhereAsync(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayMatchAward.Count;
        }
    }
}
