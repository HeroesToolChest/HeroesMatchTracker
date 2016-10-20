using HeroesParserData.DataQueries;
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
                {
                    string columnName = prop.Name.Split('_')[1];
                    if (!columnName.Contains("Ticks"))
                        ColumnNames.Add(columnName);
                }
            }
        }

        protected override void ReadDataTop()
        {
            ReplayMatchTeamExperience = new ObservableCollection<ReplayMatchTeamExperience>(Query.MatchTeamExperience.ReadTopRecords(100));
            RowsReturned = ReplayMatchTeamExperience.Count;
        }

        protected override void ReadDataLast()
        {
            ReplayMatchTeamExperience = new ObservableCollection<ReplayMatchTeamExperience>(Query.MatchTeamExperience.ReadLastRecords(100));
            RowsReturned = ReplayMatchTeamExperience.Count;
        }

        protected override void ReadDataCustomTop()
        {
            ReplayMatchTeamExperience = new ObservableCollection<ReplayMatchTeamExperience>(Query.MatchTeamExperience.ReadRecordsCustomTop(SelectedNumber, SelectedTopColumnName, SelectedTopOrderBy));
            RowsReturned = ReplayMatchTeamExperience.Count;
        }

        protected override void ReadDataWhere()
        {
            ReplayMatchTeamExperience = new ObservableCollection<ReplayMatchTeamExperience>(Query.MatchTeamExperience.ReadRecordsWhere(SelectedWhereColumnName, SelectedOperand, TextBoxSelectWhere));
            RowsReturned = ReplayMatchTeamExperience.Count;
        }
    }
}
