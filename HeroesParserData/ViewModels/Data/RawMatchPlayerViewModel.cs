using HeroesParserData.DataQueries;
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
                RaisePropertyChangedEvent(nameof(ReplayMatchPlayer));
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

        protected override void ReadDataTop()
        {
            ReplayMatchPlayer = new ObservableCollection<ReplayMatchPlayer>(Query.MatchPlayer.ReadTopRecords(100));
            RowsReturned = ReplayMatchPlayer.Count;

        }

        protected override void ReadDataLast()
        {
            ReplayMatchPlayer = new ObservableCollection<ReplayMatchPlayer>(Query.MatchPlayer.ReadLastRecords(100));
            RowsReturned = ReplayMatchPlayer.Count;
        }

        protected override void ReadDataCustomTop()
        {
            ReplayMatchPlayer = new ObservableCollection<ReplayMatchPlayer>(Query.MatchPlayer.ReadRecordsCustomTop(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayMatchPlayer.Count;
        }

        protected override void ReadDataWhere()
        {
            ReplayMatchPlayer = new ObservableCollection<ReplayMatchPlayer>(Query.MatchPlayer.ReadRecordsWhere(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayMatchPlayer.Count;
        }
    }
}
