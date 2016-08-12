using HeroesParserData.DataQueries.ReplayData;
using HeroesParserData.Models.DbModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Data
{
    public class RawMatchTeamExperienceViewModel : RawDataContext
    {
        private ObservableCollection<ReplayMatchTeamExperience> _replayMatchTeamExperience = new ObservableCollection<ReplayMatchTeamExperience>();

        public ObservableCollection<ReplayMatchTeamExperience> ReplayMatchTeamExperience
        {
            get
            {
                return _replayMatchTeamExperience;
            }
            set
            {
                _replayMatchTeamExperience = value;
                RaisePropertyChangedEvent(nameof(ReplayMatchTeamExperience));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RawMatchTeamExperienceViewModel()
            : base()
        {
            AddListColumnNames();
        }

        private void AddListColumnNames()
        {
            ReplayMatchTeamExperience r = new ReplayMatchTeamExperience();

            foreach (var prop in r.GetType().GetMethods())
            {
                if (prop.IsVirtual == false && prop.ReturnType.Name == "Void")
                    ColumnNames.Add(prop.Name.Split('_')[1]);
            }
        }

        protected override async Task ReadDataTop()
        {
            ReplayMatchTeamExperience = new ObservableCollection<ReplayMatchTeamExperience>(await Query.MatchTeamExperience.ReadTopRecordsAsync(100));
            RowsReturned = ReplayMatchTeamExperience.Count;
        }

        protected override async Task ReadDataLast()
        {
            ReplayMatchTeamExperience = new ObservableCollection<ReplayMatchTeamExperience>(await Query.MatchTeamExperience.ReadLastRecordsAsync(100));
            RowsReturned = ReplayMatchTeamExperience.Count;
        }

        protected override async Task ReadDataCustomTop()
        {
            ReplayMatchTeamExperience = new ObservableCollection<ReplayMatchTeamExperience>(await Query.MatchTeamExperience.ReadRecordsCustomTopAsync(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayMatchTeamExperience.Count;
        }

        protected override async Task ReadDataWhere()
        {
            ReplayMatchTeamExperience = new ObservableCollection<ReplayMatchTeamExperience>(await Query.MatchTeamExperience.ReadRecordsWhereAsync(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayMatchTeamExperience.Count;
        }
    }
}
