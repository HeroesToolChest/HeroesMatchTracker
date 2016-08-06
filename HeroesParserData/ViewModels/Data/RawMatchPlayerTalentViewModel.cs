using HeroesParserData.DataQueries.ReplayData;
using HeroesParserData.Models.DbModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Data
{
    public class RawMatchPlayerTalentViewModel : RawDataContext
    {
        private ObservableCollection<ReplayMatchPlayerTalent> _replayMatchPlayerTalent = new ObservableCollection<ReplayMatchPlayerTalent>();

        public ObservableCollection<ReplayMatchPlayerTalent> ReplayMatchPlayerTalent
        {
            get
            {
                return _replayMatchPlayerTalent;
            }
            set
            {
                _replayMatchPlayerTalent = value;
                RaisePropertyChangedEvent(nameof(ReplayMatchPlayerTalent));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RawMatchPlayerTalentViewModel()
            : base()
        {
            AddListColumnNames();
        }

        private void AddListColumnNames()
        {
            ReplayMatchPlayerTalent r = new ReplayMatchPlayerTalent();

            foreach (var prop in r.GetType().GetMethods())
            {
                if (prop.IsVirtual == false && prop.ReturnType.Name == "Void")
                    ColumnNames.Add(prop.Name.Split('_')[1]);
            }
        }

        protected override async Task ReadDataTop()
        {
            ReplayMatchPlayerTalent = new ObservableCollection<ReplayMatchPlayerTalent>(await Query.MatchPlayerTalent.ReadTopRecordsAsync(100));
            RowsReturned = ReplayMatchPlayerTalent.Count;

        }

        protected override async Task ReadDataLast()
        {
            ReplayMatchPlayerTalent = new ObservableCollection<ReplayMatchPlayerTalent>(await Query.MatchPlayerTalent.ReadLastRecordsAsync(100));
            RowsReturned = ReplayMatchPlayerTalent.Count;

        }

        protected override async Task ReadDataCustomTop()
        {
            ReplayMatchPlayerTalent = new ObservableCollection<ReplayMatchPlayerTalent>(await Query.MatchPlayerTalent.ReadRecordsCustomTopAsync(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayMatchPlayerTalent.Count;

        }

        protected override async Task ReadDataWhere()
        {
            ReplayMatchPlayerTalent = new ObservableCollection<ReplayMatchPlayerTalent>(await Query.MatchPlayerTalent.ReadRecordsWhereAsync(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayMatchPlayerTalent.Count;
        }
    }
}
