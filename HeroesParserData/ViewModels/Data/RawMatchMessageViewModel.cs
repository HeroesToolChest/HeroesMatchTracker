using HeroesParserData.DataQueries.ReplayData;
using HeroesParserData.Models.DbModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Data
{
    public class RawMatchMessageViewModel : RawDataContext
    {
        private ObservableCollection<ReplayMatchMessage> _replayMatchMessage = new ObservableCollection<ReplayMatchMessage>();

        public ObservableCollection<ReplayMatchMessage> ReplayMatchMessage
        {
            get
            {
                return _replayMatchMessage;
            }
            set
            {
                _replayMatchMessage = value;
                RaisePropertyChangedEvent(nameof(ReplayMatchMessage));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RawMatchMessageViewModel()
            : base()
        {
            AddListColumnNames();
        }

        private void AddListColumnNames()
        {
            ReplayMatchMessage r = new ReplayMatchMessage();

            foreach (var prop in r.GetType().GetMethods())
            {
                if (prop.IsVirtual == false && prop.ReturnType.Name == "Void")
                    ColumnNames.Add(prop.Name.Split('_')[1]);
            }
        }

        protected override async Task ReadDataTop()
        {
            ReplayMatchMessage = new ObservableCollection<ReplayMatchMessage>(await Query.MatchMessage.ReadTopRecordsAsync(100));
            RowsReturned = ReplayMatchMessage.Count;
        }

        protected override async Task ReadDataLast()
        {
            ReplayMatchMessage = new ObservableCollection<ReplayMatchMessage>(await Query.MatchMessage.ReadLastRecordsAsync(100));
            RowsReturned = ReplayMatchMessage.Count;
        }

        protected override async Task ReadDataCustomTop()
        {
            ReplayMatchMessage = new ObservableCollection<ReplayMatchMessage>(await Query.MatchMessage.ReadRecordsCustomTopAsync(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayMatchMessage.Count;
        }

        protected override async Task ReadDataWhere()
        {
            ReplayMatchMessage = new ObservableCollection<ReplayMatchMessage>(await Query.MatchMessage.ReadRecordsWhereAsync(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayMatchMessage.Count;
        }
    }
}
