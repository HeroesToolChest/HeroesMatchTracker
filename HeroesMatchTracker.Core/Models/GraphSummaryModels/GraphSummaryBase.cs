using GalaSoft.MvvmLight;

namespace HeroesMatchTracker.Core.Models.GraphSummaryModels
{
    public abstract class GraphSummaryBase : ObservableObject
    {
        public string GraphTeam1Title { get; private set; } = "Team 1";
        public string GraphTeam2Title { get; private set; } = "Team 2";

        public abstract void Dispose();

        protected void SetWinner(bool isTeam1Winner)
        {
            if (isTeam1Winner)
                GraphTeam1Title = "Team 1 (WINNER)";
            else
                GraphTeam2Title = "Team 2 (WINNER)";
        }

        protected void ClearWinner()
        {
            GraphTeam1Title = "Team 1";
            GraphTeam2Title = "Team 2";
        }
    }
}
