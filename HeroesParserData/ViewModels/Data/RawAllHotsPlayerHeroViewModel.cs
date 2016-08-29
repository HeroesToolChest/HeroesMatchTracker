using HeroesParserData.DataQueries.ReplayData;
using HeroesParserData.Models.DbModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Data
{
    public class RawAllHotsPlayerHeroViewModel : RawDataContext
    {
        private ObservableCollection<ReplayAllHotsPlayerHero> _replayAllHotsPlayerHero = new ObservableCollection<ReplayAllHotsPlayerHero>();

        public ObservableCollection<ReplayAllHotsPlayerHero> ReplayAllHotsPlayerHero
        {
            get
            {
                return _replayAllHotsPlayerHero;
            }
            set
            {
                _replayAllHotsPlayerHero = value;
                RaisePropertyChangedEvent(nameof(ReplayAllHotsPlayerHero));
            }
        }

        public RawAllHotsPlayerHeroViewModel()
            : base()
        {
            AddListColumnNames();
        }

        protected void AddListColumnNames()
        {
            ReplayAllHotsPlayerHero r = new ReplayAllHotsPlayerHero();

            foreach (var prop in r.GetType().GetMethods())
            {
                if (prop.IsVirtual == false && prop.ReturnType.Name == "Void")
                    ColumnNames.Add(prop.Name.Split('_')[1]);
            }
        }

        protected override async Task ReadDataTop()
        {
            ReplayAllHotsPlayerHero = new ObservableCollection<ReplayAllHotsPlayerHero>(await Query.HotsPlayerHero.ReadTopRecordsAsync(100));
            RowsReturned = ReplayAllHotsPlayerHero.Count;
        }

        protected override async Task ReadDataLast()
        {
            ReplayAllHotsPlayerHero = new ObservableCollection<ReplayAllHotsPlayerHero>(await Query.HotsPlayerHero.ReadLastRecordsAsync(100));
            RowsReturned = ReplayAllHotsPlayerHero.Count;
        }

        protected override async Task ReadDataCustomTop()
        {
            ReplayAllHotsPlayerHero = new ObservableCollection<ReplayAllHotsPlayerHero>(await Query.HotsPlayerHero.ReadRecordsCustomTopAsync(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayAllHotsPlayerHero.Count;
        }

        protected override async Task ReadDataWhere()
        {
            ReplayAllHotsPlayerHero = new ObservableCollection<ReplayAllHotsPlayerHero>(await Query.HotsPlayerHero.ReadRecordsWhereAsync(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayAllHotsPlayerHero.Count;
        }
    }
}
