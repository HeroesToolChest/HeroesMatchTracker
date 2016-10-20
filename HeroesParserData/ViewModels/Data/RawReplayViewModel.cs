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

        protected override void ReadDataTop()
        {
            Replays = new ObservableCollection<Replay>(Query.Replay.ReadTopRecords(100));
            RowsReturned = Replays.Count;
        }

        protected override void ReadDataLast()
        {
            Replays = new ObservableCollection<Replay>(Query.Replay.ReadLastRecords(100));
            RowsReturned = Replays.Count;
        }

        protected override void ReadDataCustomTop()
        {
            Replays = new ObservableCollection<Replay>(Query.Replay.ReadRecordsCustomTop(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = Replays.Count;
        }

        protected override void ReadDataWhere()
        {
            Replays = new ObservableCollection<Replay>(Query.Replay.ReadRecordsWhere(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = Replays.Count;
        }
    }
}
