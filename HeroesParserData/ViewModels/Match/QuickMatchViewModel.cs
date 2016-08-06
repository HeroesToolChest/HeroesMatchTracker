using HeroesParserData.DataQueries.ReplayData;
using HeroesParserData.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Match
{
    public class QuickMatchViewModel : MatchContext
    {
        public QuickMatchViewModel()
            :base()
        {

        }

        protected override async Task RefreshExecute()
        {
            await QueryMatchList();
        }

        private async Task QueryMatchList()
        {
            MatchList = new ObservableCollection<Replay>(await Query.Replay.ReadGameModeRecordsAsync("QuickMatch"));
        }
    }
}
