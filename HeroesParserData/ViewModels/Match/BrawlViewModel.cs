using GalaSoft.MvvmLight.Messaging;
using Heroes.ReplayParser;
using HeroesParserData.DataQueries;
using HeroesParserData.Messages;
using System.Collections.ObjectModel;
using System.Linq;

namespace HeroesParserData.ViewModels.Match
{
    public class BrawlViewModel : MatchOverviewContext
    {
        public BrawlViewModel()
            :base()
        {
            Messenger.Default.Register<GameModesMessage>(this, (action) => ReceiveMessage(action));
        }

        protected override void ExecuteLoadMatchListCommmand()
        {
            MatchListCollection = new ObservableCollection<Models.DbModels.Replay>(Query.Replay.ReadGameModeRecords(GameMode.Brawl, this));
            RowsReturned = MatchListCollection.Count;
        }

        protected override void ExecuteShowMatchSummaryCommand()
        {
            if (SelectedReplay == null)
                return;

            Messenger.Default.Send(new MatchSummaryMessage
            {
                Replay = SelectedReplay,
                MatchSummary = MatchSummary.Brawl,
                MatchList = MatchListCollection.ToList(),
                Trigger = Trigger.Open
            });
        }

        protected override void ReceiveMessage(GameModesMessage action)
        {
            if (action.GameModesTab == GameModesTab.Brawl)
            {
                if (!string.IsNullOrEmpty(action.SelectedCharacter))
                    SelectedCharacter = action.SelectedCharacter;
                if (!string.IsNullOrEmpty(action.SelectedBattleTagName))
                    SelectedPlayerBattleTag = action.SelectedBattleTagName;
                if (!string.IsNullOrEmpty(action.SelectedCharacter) && !string.IsNullOrEmpty(action.SelectedBattleTagName))
                    IsGivenBattleTagOnlyChecked = true;
            }
        }
    }
}
