using HeroesParserData.DataQueries;
using HeroesParserData.Models.DbModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
namespace HeroesParserData.ViewModels.Data
{
    public class RawRenamedPlayerViewModel : RawDataContext
    {
        private ObservableCollection<ReplayRenamedPlayer> _replayRenamedPlayer = new ObservableCollection<ReplayRenamedPlayer>();

        public ObservableCollection<ReplayRenamedPlayer> ReplayRenamedPlayer
        {
            get
            {
                return _replayRenamedPlayer;
            }
            set
            {
                _replayRenamedPlayer = value;
                RaisePropertyChangedEvent(nameof(ReplayRenamedPlayer));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RawRenamedPlayerViewModel()
            : base()
        {
            AddListColumnNames();
        }

        private void AddListColumnNames()
        {
            ReplayRenamedPlayer r = new ReplayRenamedPlayer();

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
            ReplayRenamedPlayer = new ObservableCollection<ReplayRenamedPlayer>(Query.RenamedPlayer.ReadTopRecords(100));
            RowsReturned = ReplayRenamedPlayer.Count;
        }

        protected override void ReadDataLast()
        {
            ReplayRenamedPlayer = new ObservableCollection<ReplayRenamedPlayer>(Query.RenamedPlayer.ReadLastRecords(100));
            RowsReturned = ReplayRenamedPlayer.Count;
        }

        protected override void ReadDataCustomTop()
        {
            ReplayRenamedPlayer = new ObservableCollection<ReplayRenamedPlayer>(Query.RenamedPlayer.ReadRecordsCustomTop(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayRenamedPlayer.Count;
        }

        protected override void ReadDataWhere()
        {
            ReplayRenamedPlayer = new ObservableCollection<ReplayRenamedPlayer>(Query.RenamedPlayer.ReadRecordsWhere(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayRenamedPlayer.Count;
        }
    }
}
