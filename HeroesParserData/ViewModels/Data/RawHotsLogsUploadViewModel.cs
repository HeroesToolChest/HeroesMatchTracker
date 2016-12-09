using HeroesParserData.DataQueries;
using HeroesParserData.Models.DbModels;
using System.Collections.ObjectModel;

namespace HeroesParserData.ViewModels.Data
{
    public class RawHotsLogsUploadViewModel : RawDataContext
    {
        private ObservableCollection<ReplayHotsLogsUpload> _replayHotsLogsUpload = new ObservableCollection<ReplayHotsLogsUpload>();

        public ObservableCollection<ReplayHotsLogsUpload> ReplayHotsLogsUploadCollection
        {
            get
            {
                return _replayHotsLogsUpload;
            }
            set
            {
                _replayHotsLogsUpload = value;
                RaisePropertyChangedEvent(nameof(ReplayHotsLogsUploadCollection));
            }
        }

        public RawHotsLogsUploadViewModel()
            : base()
        {
            AddListColumnNames();
        }

        protected void AddListColumnNames()
        {
            ReplayHotsLogsUpload r = new ReplayHotsLogsUpload();

            foreach (var prop in r.GetType().GetMethods())
            {
                if (prop.IsVirtual == false && prop.ReturnType.Name == "Void")
                    ColumnNames.Add(prop.Name.Split('_')[1]);
            }
        }

        protected override void ReadDataTop()
        {
            ReplayHotsLogsUploadCollection = new ObservableCollection<ReplayHotsLogsUpload>(Query.HotsLogsUpload.ReadTopRecords(100));
            RowsReturned = ReplayHotsLogsUploadCollection.Count;
        }

        protected override void ReadDataLast()
        {
            ReplayHotsLogsUploadCollection = new ObservableCollection<ReplayHotsLogsUpload>(Query.HotsLogsUpload.ReadLastRecords(100));
            RowsReturned = ReplayHotsLogsUploadCollection.Count;
        }

        protected override void ReadDataCustomTop()
        {
            ReplayHotsLogsUploadCollection = new ObservableCollection<ReplayHotsLogsUpload>(Query.HotsLogsUpload.ReadRecordsCustomTop(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayHotsLogsUploadCollection.Count;
        }

        protected override void ReadDataWhere()
        {
            ReplayHotsLogsUploadCollection = new ObservableCollection<ReplayHotsLogsUpload>(Query.HotsLogsUpload.ReadRecordsWhere(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayHotsLogsUploadCollection.Count;
        }
    }
}
