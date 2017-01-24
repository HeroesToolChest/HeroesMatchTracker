using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Heroes.ReplayParser;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class QuickMatchViewModel : MatchesBase
    {
        public QuickMatchViewModel()
            : base(GameMode.QuickMatch)
        {
        }
    }
}
