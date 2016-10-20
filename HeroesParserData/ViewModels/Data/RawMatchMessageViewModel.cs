using HeroesParserData.DataQueries;
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
                {
                    string columnName = prop.Name.Split('_')[1];
                    if (!columnName.Contains("Ticks"))
                        ColumnNames.Add(columnName);
                }
            }
        }

        protected override void ReadDataTop()
        {
            ReplayMatchMessage = new ObservableCollection<ReplayMatchMessage>(Query.MatchMessage.ReadTopRecords(100));
            RowsReturned = ReplayMatchMessage.Count;
        }

        protected override void ReadDataLast()
        {
            ReplayMatchMessage = new ObservableCollection<ReplayMatchMessage>(Query.MatchMessage.ReadLastRecords(100));
            RowsReturned = ReplayMatchMessage.Count;
        }

        protected override void ReadDataCustomTop()
        {
            ReplayMatchMessage = new ObservableCollection<ReplayMatchMessage>(Query.MatchMessage.ReadRecordsCustomTop(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayMatchMessage.Count;
        }

        protected override void ReadDataWhere()
        {
            ReplayMatchMessage = new ObservableCollection<ReplayMatchMessage>(Query.MatchMessage.ReadRecordsWhere(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayMatchMessage.Count;
        }
    }
}
