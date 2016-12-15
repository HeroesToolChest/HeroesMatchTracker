using GalaSoft.MvvmLight.Messaging;
using HeroesIcons;
using HeroesParserData.Messages;
using HeroesParserData.Models.DbModels;
using HeroesParserData.Models.MatchModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace HeroesParserData.ViewModels.Match
{
    public class MatchContextBase : ViewModelBase
    {
        private MatchPlayerInfoBase _selectedDataPlayer;

        protected readonly Color Team1BackColor = Color.FromRgb(179, 179, 255);
        protected readonly Color Team2BackColor = Color.FromRgb(235, 159, 159);
        protected readonly Color WinningTeamBackColor = Color.FromRgb(233, 252, 233);
        protected readonly Color LosingTeamBackColor = Colors.AliceBlue;

        protected Dictionary<int, PartyIconColor> PlayerPartyIcons { get; private set; } = new Dictionary<int, PartyIconColor>();

        public bool ShowPlayerTagNumber
        {
            get { return !UserSettings.Default.IsBattleTagHidden; }
            set
            {
                UserSettings.Default.IsBattleTagHidden = !value;
                RaisePropertyChangedEvent(nameof(ShowPlayerTagNumber));
            }
        }

        public MatchPlayerInfoBase SelectedDataPlayer
        {
            get { return _selectedDataPlayer; }
            set
            {
                _selectedDataPlayer = value;
                RaisePropertyChangedEvent(nameof(SelectedDataPlayer));
            }
        }

        protected MatchContextBase()
            :base()
        {

        }

        protected void FindPlayerParties(List<ReplayMatchPlayer> playersList)
        {
            Dictionary<long, List<int>> parties = new Dictionary<long, List<int>>();

            foreach (var player in playersList)
            {
                if (player.PartyValue != 0)
                {
                    if (!parties.ContainsKey(player.PartyValue))
                    {
                        var listOfMembers = new List<int>();
                        listOfMembers.Add(player.PlayerNumber);
                        parties.Add(player.PartyValue, listOfMembers);
                    }
                    else
                    {
                        var listOfMembers = parties[player.PartyValue];
                        listOfMembers.Add(player.PlayerNumber);
                        parties[player.PartyValue] = listOfMembers;
                    }
                }
            }

            PlayerPartyIcons = new Dictionary<int, PartyIconColor>();
            PartyIconColor color = 0;

            foreach (var party in parties)
            {
                foreach (int playerNum in party.Value)
                {
                    PlayerPartyIcons.Add(playerNum, color);
                }
                color++;
            }
        }

        protected void SetContextMenuCommands(MatchPlayerInfoBase matchPlayerInfoBase)
        {
            matchPlayerInfoBase.PlayerSearchContextMenu.HeroSearchAllMatchCommand = new DelegateCommand(() => ExecuteHeroSearchAllMatchCommand());
            matchPlayerInfoBase.PlayerSearchContextMenu.HeroSearchQuickMatchCommand = new DelegateCommand(() => ExecuteHeroSearchQuickMatchCommand());
            matchPlayerInfoBase.PlayerSearchContextMenu.HeroSearchUnrankedDraftCommand = new DelegateCommand(() => ExecuteHeroSearchUnrankedDraftCommand());
            matchPlayerInfoBase.PlayerSearchContextMenu.HeroSearchHeroLeagueCommand = new DelegateCommand(() => ExecuteHeroSearchHeroLeagueCommand());
            matchPlayerInfoBase.PlayerSearchContextMenu.HeroSearchTeamLeagueCommand = new DelegateCommand(() => ExecuteHeroSearchTeamLeagueCommand());
            matchPlayerInfoBase.PlayerSearchContextMenu.HeroSearchBrawlCommand = new DelegateCommand(() => ExecuteHeroSearchBrawlCommand());
            matchPlayerInfoBase.PlayerSearchContextMenu.HeroSearchCustomGameCommand = new DelegateCommand(() => ExecuteHeroSearchCustomGameCommand());

            matchPlayerInfoBase.PlayerSearchContextMenu.PlayerSearchAllMatchCommand = new DelegateCommand(() => ExecutePlayerSearchAllMatchCommand());
            matchPlayerInfoBase.PlayerSearchContextMenu.PlayerSearchQuickMatchCommand = new DelegateCommand(() => ExecutePlayerSearchQuickMatchCommand());
            matchPlayerInfoBase.PlayerSearchContextMenu.PlayerSearchUnrankedDraftCommand = new DelegateCommand(() => ExecutePlayerSearchUnrankedDraftCommand());
            matchPlayerInfoBase.PlayerSearchContextMenu.PlayerSearchHeroLeagueCommand = new DelegateCommand(() => ExecutePlayerSearchHeroLeagueCommand());
            matchPlayerInfoBase.PlayerSearchContextMenu.PlayerSearchTeamLeagueCommand = new DelegateCommand(() => ExecutePlayerSearchTeamLeagueCommand());
            matchPlayerInfoBase.PlayerSearchContextMenu.PlayerSearchBrawlCommand = new DelegateCommand(() => ExecutePlayerSearchBrawlCommand());
            matchPlayerInfoBase.PlayerSearchContextMenu.PlayerSearchCustomGameCommand = new DelegateCommand(() => ExecutePlayerSearchCustomGameCommand());

            matchPlayerInfoBase.PlayerSearchContextMenu.PlayerHeroSearchAllMatchCommand = new DelegateCommand(() => ExecutePlayerHeroSearchAllMatchCommand());
            matchPlayerInfoBase.PlayerSearchContextMenu.PlayerHeroSearchQuickMatchCommand = new DelegateCommand(() => ExecutePlayerHeroSearchQuickMatchCommand());
            matchPlayerInfoBase.PlayerSearchContextMenu.PlayerHeroSearchUnrankedDraftCommand = new DelegateCommand(() => ExecutePlayerHeroSearchUnrankedDraftCommand());
            matchPlayerInfoBase.PlayerSearchContextMenu.PlayerHeroSearchHeroLeagueCommand = new DelegateCommand(() => ExecutePlayerHeroSearchHeroLeagueCommand());
            matchPlayerInfoBase.PlayerSearchContextMenu.PlayerHeroSearchTeamLeagueCommand = new DelegateCommand(() => ExecutePlayerHeroSearchTeamLeagueCommand());
            matchPlayerInfoBase.PlayerSearchContextMenu.PlayerHeroSearchBrawlCommand = new DelegateCommand(() => ExecutePlayerHeroSearchBrawlCommand());
            matchPlayerInfoBase.PlayerSearchContextMenu.PlayerHeroSearchCustomGameCommand = new DelegateCommand(() => ExecutePlayerHeroSearchCustomGameCommand());

            matchPlayerInfoBase.PlayerSearchContextMenu.CopyHeroNameToClipboardCommand = new DelegateCommand(() => ExecuteCopyHeroNameToClipboardCommand());
            matchPlayerInfoBase.PlayerSearchContextMenu.CopyPlayerNameToClipboardCommand = new DelegateCommand(() => ExecuteCopyPlayerTagNameToClipboardCommand());
            matchPlayerInfoBase.PlayerSearchContextMenu.CopyHeroWithPlayerNameToClipboardCommand = new DelegateCommand(() => ExecuteCopyHerowithPlayerTagNameToClipboardCommand());
        }

        // -------- Hero Search
        #region Execute Hero Search
        private void ExecuteHeroSearchAllMatchCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.Matches, SelectedCharacter = SelectedDataPlayer.CharacterName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }

        private void ExecuteHeroSearchQuickMatchCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.QuickMatch, SelectedCharacter = SelectedDataPlayer.CharacterName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }

        private void ExecuteHeroSearchUnrankedDraftCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.UnrankedDraft, SelectedCharacter = SelectedDataPlayer.CharacterName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }

        private void ExecuteHeroSearchHeroLeagueCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.HeroLeague, SelectedCharacter = SelectedDataPlayer.CharacterName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }

        private void ExecuteHeroSearchTeamLeagueCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.TeamLeague, SelectedCharacter = SelectedDataPlayer.CharacterName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }

        private void ExecuteHeroSearchBrawlCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.Brawl, SelectedCharacter = SelectedDataPlayer.CharacterName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }

        private void ExecuteHeroSearchCustomGameCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.CustomGame, SelectedCharacter = SelectedDataPlayer.CharacterName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }
        #endregion Execute Hero Search

        // -------- Player Search
        #region Execute Player Search
        private void ExecutePlayerSearchAllMatchCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.Matches, SelectedBattleTagName = SelectedDataPlayer.PlayerName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }

        private void ExecutePlayerSearchQuickMatchCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.QuickMatch, SelectedBattleTagName = SelectedDataPlayer.PlayerName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }

        private void ExecutePlayerSearchUnrankedDraftCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.UnrankedDraft, SelectedBattleTagName = SelectedDataPlayer.PlayerName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }

        private void ExecutePlayerSearchHeroLeagueCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.HeroLeague, SelectedBattleTagName = SelectedDataPlayer.PlayerName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }

        private void ExecutePlayerSearchTeamLeagueCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.TeamLeague, SelectedBattleTagName = SelectedDataPlayer.PlayerName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }

        private void ExecutePlayerSearchBrawlCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.Brawl, SelectedBattleTagName = SelectedDataPlayer.PlayerName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }

        private void ExecutePlayerSearchCustomGameCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.CustomGame, SelectedBattleTagName = SelectedDataPlayer.PlayerName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }
        #endregion Execute Player Search

        // -------- Player with Hero Search
        #region Execute Player with Hero
        private void ExecutePlayerHeroSearchAllMatchCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.Matches, SelectedCharacter = SelectedDataPlayer.CharacterName, SelectedBattleTagName = SelectedDataPlayer.PlayerName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }

        private void ExecutePlayerHeroSearchQuickMatchCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.QuickMatch, SelectedCharacter = SelectedDataPlayer.CharacterName, SelectedBattleTagName = SelectedDataPlayer.PlayerName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }

        private void ExecutePlayerHeroSearchUnrankedDraftCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.UnrankedDraft, SelectedCharacter = SelectedDataPlayer.CharacterName, SelectedBattleTagName = SelectedDataPlayer.PlayerName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }

        private void ExecutePlayerHeroSearchHeroLeagueCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.HeroLeague, SelectedCharacter = SelectedDataPlayer.CharacterName, SelectedBattleTagName = SelectedDataPlayer.PlayerName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }

        private void ExecutePlayerHeroSearchTeamLeagueCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.TeamLeague, SelectedCharacter = SelectedDataPlayer.CharacterName, SelectedBattleTagName = SelectedDataPlayer.PlayerName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }

        private void ExecutePlayerHeroSearchBrawlCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.Brawl, SelectedCharacter = SelectedDataPlayer.CharacterName, SelectedBattleTagName = SelectedDataPlayer.PlayerName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }

        private void ExecutePlayerHeroSearchCustomGameCommand()
        {
            Messenger.Default.Send(new MainTabMessage { MainTab = MainTab.GameModes });
            Messenger.Default.Send(new GameModesMessage { GameModesTab = GameModesTab.CustomGame, SelectedCharacter = SelectedDataPlayer.CharacterName, SelectedBattleTagName = SelectedDataPlayer.PlayerName });
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.All, Trigger = Messages.Trigger.Close });
        }
        #endregion Execute Player with Hero

        #region Execute Copy Commands
        private void ExecuteCopyHeroNameToClipboardCommand()
        {
            Clipboard.SetText(SelectedDataPlayer.CharacterName);
        }

        private void ExecuteCopyPlayerTagNameToClipboardCommand()
        {
            Clipboard.SetText(SelectedDataPlayer.PlayerName);
        }

        private void ExecuteCopyHerowithPlayerTagNameToClipboardCommand()
        {
            Clipboard.SetText($"{SelectedDataPlayer.CharacterName} - {SelectedDataPlayer.PlayerName}");
        }
        #endregion Execute Copy Commands
    }
}
