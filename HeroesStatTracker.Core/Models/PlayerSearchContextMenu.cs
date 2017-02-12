using GalaSoft.MvvmLight.CommandWpf;

namespace HeroesStatTracker.Core.Models
{
    public class PlayerSearchContextMenu
    {
        public RelayCommand HeroSearchAllMatchCommand { get; set; }
        public RelayCommand HeroSearchQuickMatchCommand { get; set; }
        public RelayCommand HeroSearchUnrankedDraftCommand { get; set; }
        public RelayCommand HeroSearchHeroLeagueCommand { get; set; }
        public RelayCommand HeroSearchTeamLeagueCommand { get; set; }
        public RelayCommand HeroSearchBrawlCommand { get; set; }
        public RelayCommand HeroSearchCustomGameCommand { get; set; }

        public RelayCommand PlayerSearchAllMatchCommand { get; set; }
        public RelayCommand PlayerSearchQuickMatchCommand { get; set; }
        public RelayCommand PlayerSearchUnrankedDraftCommand { get; set; }
        public RelayCommand PlayerSearchHeroLeagueCommand { get; set; }
        public RelayCommand PlayerSearchTeamLeagueCommand { get; set; }
        public RelayCommand PlayerSearchBrawlCommand { get; set; }
        public RelayCommand PlayerSearchCustomGameCommand { get; set; }

        public RelayCommand PlayerHeroSearchAllMatchCommand { get; set; }
        public RelayCommand PlayerHeroSearchQuickMatchCommand { get; set; }
        public RelayCommand PlayerHeroSearchUnrankedDraftCommand { get; set; }
        public RelayCommand PlayerHeroSearchHeroLeagueCommand { get; set; }
        public RelayCommand PlayerHeroSearchTeamLeagueCommand { get; set; }
        public RelayCommand PlayerHeroSearchBrawlCommand { get; set; }
        public RelayCommand PlayerHeroSearchCustomGameCommand { get; set; }

        public RelayCommand CopyHeroNameToClipboardCommand { get; set; }
        public RelayCommand CopyPlayerNameToClipboardCommand { get; set; }
        public RelayCommand CopyHeroWithPlayerNameToClipboardCommand { get; set; }
    }
}
