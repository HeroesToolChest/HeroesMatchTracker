using System.Windows.Input;

namespace HeroesParserData.Models
{
    public class PlayerSearchContextMenu
    {
        public ICommand HeroSearchQuickMatchCommand { get; set; }
        public ICommand HeroSearchUnrankedDraftCommand { get; set; }
        public ICommand HeroSearchHeroLeagueCommand { get; set; }
        public ICommand HeroSearchTeamLeagueCommand { get; set; }
        public ICommand HeroSearchBrawlCommand { get; set; }
        public ICommand HeroSearchCustomGameCommand { get; set; }

        public ICommand PlayerSearchQuickMatchCommand { get; set; }
        public ICommand PlayerSearchUnrankedDraftCommand { get; set; }
        public ICommand PlayerSearchHeroLeagueCommand { get; set; }
        public ICommand PlayerSearchTeamLeagueCommand { get; set; }
        public ICommand PlayerSearchBrawlCommand { get; set; }
        public ICommand PlayerSearchCustomGameCommand { get; set; }

        public ICommand PlayerHeroSearchQuickMatchCommand { get; set; }
        public ICommand PlayerHeroSearchUnrankedDraftCommand { get; set; }
        public ICommand PlayerHeroSearchHeroLeagueCommand { get; set; }
        public ICommand PlayerHeroSearchTeamLeagueCommand { get; set; }
        public ICommand PlayerHeroSearchBrawlCommand { get; set; }
        public ICommand PlayerHeroSearchCustomGameCommand { get; set; }

        public ICommand CopyHeroNameToClipboardCommand { get; set; }
        public ICommand CopyPlayerNameToClipboardCommand { get; set; }
        public ICommand CopyHeroWithPlayerNameToClipboardCommand { get; set; }
    }
}
