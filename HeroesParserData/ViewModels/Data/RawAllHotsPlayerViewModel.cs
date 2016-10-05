using HeroesParserData.DataQueries;
using HeroesParserData.Models.DbModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Data
{
    public class RawAllHotsPlayerViewModel : RawDataContext
    {
        private ObservableCollection<ReplayAllHotsPlayer> _replayAllHotsPlayer = new ObservableCollection<ReplayAllHotsPlayer>();

        public ObservableCollection<ReplayAllHotsPlayer> ReplayAllHotsPlayer
        {
            get
            {
                return _replayAllHotsPlayer;
            }
            set
            {
                _replayAllHotsPlayer = value;
                RaisePropertyChangedEvent(nameof(ReplayAllHotsPlayer));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RawAllHotsPlayerViewModel()
            : base()
        {
            AddListColumnNames();
        }

        protected void AddListColumnNames()
        {
            ReplayAllHotsPlayer r = new ReplayAllHotsPlayer();

            foreach (var prop in r.GetType().GetMethods())
            {
                if (prop.IsVirtual == false && prop.ReturnType.Name == "Void")
                    ColumnNames.Add(prop.Name.Split('_')[1]);
            }
        }

        protected override async Task ReadDataTop()
        {
            ReplayAllHotsPlayer = new ObservableCollection<ReplayAllHotsPlayer>(await Query.HotsPlayer.ReadTopRecordsAsync(100));
            RowsReturned = ReplayAllHotsPlayer.Count;
        }

        protected override async Task ReadDataLast()
        {
            ReplayAllHotsPlayer = new ObservableCollection<ReplayAllHotsPlayer>(await Query.HotsPlayer.ReadLastRecordsAsync(100));
            RowsReturned = ReplayAllHotsPlayer.Count;
        }

        protected override async Task ReadDataCustomTop()
        {
            ReplayAllHotsPlayer = new ObservableCollection<ReplayAllHotsPlayer>(await Query.HotsPlayer.ReadRecordsCustomTopAsync(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayAllHotsPlayer.Count;
        }

        protected override async Task ReadDataWhere()
        {
            ReplayAllHotsPlayer = new ObservableCollection<ReplayAllHotsPlayer>(await Query.HotsPlayer.ReadRecordsWhereAsync(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayAllHotsPlayer.Count;
        }
    }
}
