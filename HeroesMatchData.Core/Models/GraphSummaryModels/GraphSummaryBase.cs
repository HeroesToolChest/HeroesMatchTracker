using GalaSoft.MvvmLight;

namespace HeroesMatchData.Core.Models.GraphSummaryModels
{
    public class GraphSummaryBase : ObservableObject
    {
        public string GraphTeam1Title { get; private set; } = "Team 1";
        public string GraphTeam2Title { get; private set; } = "Team 2";

        protected void SetWinner(bool isTeam1Winner)
        {
            if (isTeam1Winner)
                GraphTeam1Title = "Team 1 (WINNER)";
            else
                GraphTeam2Title = "Team 2 (WINNER)";
        }
    }
}
