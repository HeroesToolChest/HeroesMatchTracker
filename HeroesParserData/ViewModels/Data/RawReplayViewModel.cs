using HeroesParserData.DataQueries;
using HeroesParserData.Models.DbModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Data
{
    public class RawReplayViewModel : RawDataContext
    {
        private ObservableCollection<Replay> _replays = new ObservableCollection<Replay>();

        public ObservableCollection<Replay> Replays
        {
            get
            {
                return _replays;
            }
            set
            {
                _replays = value;
                RaisePropertyChangedEvent(nameof(Replays));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RawReplayViewModel()
            : base()
        {
            AddListColumnNames();
        }

        private void AddListColumnNames()
        {
            Replay r = new Replay();

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
            Replays = new ObservableCollection<Replay>(await Query.Replay.ReadTopRecordsAsync(100));
            RowsReturned = Replays.Count;
        }

        protected override async Task ReadDataLast()
        {
            Replays = new ObservableCollection<Replay>(await Query.Replay.ReadLastRecordsAsync(100));
            RowsReturned = Replays.Count;
        }

        protected override async Task ReadDataCustomTop()
        {
            Replays = new ObservableCollection<Replay>(await Query.Replay.ReadRecordsCustomTopAsync(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = Replays.Count;
        }

        protected override async Task ReadDataWhere()
        {
            Replays = new ObservableCollection<Replay>(await Query.Replay.ReadRecordsWhereAsync(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = Replays.Count;
        }
    }
}
