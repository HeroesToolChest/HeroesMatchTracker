using HeroesParserData.DataQueries;
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

        protected override void ReadDataTop()
        {
            ReplayAllHotsPlayerHero = new ObservableCollection<ReplayAllHotsPlayerHero>(Query.HotsPlayerHero.ReadTopRecords(100));
            RowsReturned = ReplayAllHotsPlayerHero.Count;
        }

        protected override void ReadDataLast()
        {
            ReplayAllHotsPlayerHero = new ObservableCollection<ReplayAllHotsPlayerHero>(Query.HotsPlayerHero.ReadLastRecords(100));
            RowsReturned = ReplayAllHotsPlayerHero.Count;
        }

        protected override void ReadDataCustomTop()
        {
            ReplayAllHotsPlayerHero = new ObservableCollection<ReplayAllHotsPlayerHero>(Query.HotsPlayerHero.ReadRecordsCustomTop(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayAllHotsPlayerHero.Count;
        }

        protected override void ReadDataWhere()
        {
            ReplayAllHotsPlayerHero = new ObservableCollection<ReplayAllHotsPlayerHero>(Query.HotsPlayerHero.ReadRecordsWhere(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayAllHotsPlayerHero.Count;
        }
    }
}
