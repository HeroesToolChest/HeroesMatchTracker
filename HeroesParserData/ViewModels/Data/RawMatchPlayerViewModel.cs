using HeroesParserData.DataQueries.ReplayData;
using HeroesParserData.Models.DbModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Data
{
    public class RawMatchPlayerViewModel : RawDataContext
    {
        private ObservableCollection<ReplayMatchPlayer> _replayMatchPlayer = new ObservableCollection<ReplayMatchPlayer>();

        public ObservableCollection<ReplayMatchPlayer> ReplayMatchPlayer
        {
            get
            {
                return _replayMatchPlayer;
            }
            set
            {
                _replayMatchPlayer = value;
                RaisePropertyChangedEvent("ReplayMatchPlayer");
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RawMatchPlayerViewModel()
            : base()
        {
            AddListColumnNames();
        }

        private void AddListColumnNames()
        {
            ReplayMatchPlayer r = new ReplayMatchPlayer();

            foreach (var prop in r.GetType().GetMethods())
            {
                if (prop.IsVirtual == false && prop.ReturnType.Name == "Void")
                    ColumnNames.Add(prop.Name.Split('_')[1]);
            }
        }

        protected override async Task ReadDataTop()
        {
            ReplayMatchPlayer = new ObservableCollection<ReplayMatchPlayer>(await Query.MatchPlayer.ReadTopRecordsAsync(100));
            RowsReturned = ReplayMatchPlayer.Count;

        }

        protected override async Task ReadDataLast()
        {
            ReplayMatchPlayer = new ObservableCollection<ReplayMatchPlayer>(await Query.MatchPlayer.ReadLastRecordsAsync(100));
            RowsReturned = ReplayMatchPlayer.Count;
        }

        protected override async Task ReadDataCustomTop()
        {
            ReplayMatchPlayer = new ObservableCollection<ReplayMatchPlayer>(await Query.MatchPlayer.ReadRecordsCustomTopAsync(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayMatchPlayer.Count;
        }

        protected override async Task ReadDataWhere()
        {
            ReplayMatchPlayer = new ObservableCollection<ReplayMatchPlayer>(await Query.MatchPlayer.ReadRecordsWhereAsync(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayMatchPlayer.Count;
        }
    }
}
