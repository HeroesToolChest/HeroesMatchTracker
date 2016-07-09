using HeroesParserData.DataQueries.ReplayData;
using HeroesParserData.Models.DbModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Data
{
    public class RawMatchChatViewModel : RawDataContext
    {
        private ObservableCollection<ReplayMatchChat> _replayMatchChat = new ObservableCollection<ReplayMatchChat>();

        public ObservableCollection<ReplayMatchChat> ReplayMatchChat
        {
            get
            {
                return _replayMatchChat;
            }
            set
            {
                _replayMatchChat = value;
                RaisePropertyChangedEvent("ReplayMatchChat");
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RawMatchChatViewModel()
            : base()
        {
            AddListColumnNames();
        }

        private void AddListColumnNames()
        {
            ReplayMatchChat r = new ReplayMatchChat();

            foreach (var prop in r.GetType().GetMethods())
            {
                if (prop.IsVirtual == false && prop.ReturnType.Name == "Void")
                    ColumnNames.Add(prop.Name.Split('_')[1]);
            }
        }

        protected override async Task ReadDataTop()
        {
            ReplayMatchChat = new ObservableCollection<ReplayMatchChat>(await Query.MatchChat.ReadTop100RecordsAsync());
            RowsReturned = ReplayMatchChat.Count;
        }

        protected override async Task ReadDataLast()
        {
            ReplayMatchChat = new ObservableCollection<ReplayMatchChat>(await Query.MatchChat.ReadLast100RecordsAsync());
            RowsReturned = ReplayMatchChat.Count;
        }

        protected override async Task ReadDataCustomTop()
        {
            ReplayMatchChat = new ObservableCollection<ReplayMatchChat>(await Query.MatchChat.ReadRecordsCustomTopAsync(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayMatchChat.Count;
        }

        protected override async Task ReadDataWhere()
        {
            ReplayMatchChat = new ObservableCollection<ReplayMatchChat>(await Query.MatchChat.ReadRecordsWhereAsync(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayMatchChat.Count;
        }
    }
}
