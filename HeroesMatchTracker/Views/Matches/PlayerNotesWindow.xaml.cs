using HeroesMatchTracker.Core.ViewModels.Matches;
using HeroesMatchTracker.Data;
using HeroesMatchTracker.Data.Models.Replays;
using MahApps.Metro.Controls;

namespace HeroesMatchTracker.Views.Matches
{
    /// <summary>
    /// Interaction logic for PlayerNotesWindow.xaml
    /// </summary>
    public partial class PlayerNotesWindow : MetroWindow
    {
        private PlayerNotesWindowViewModel PlayerNotesWindowViewModel;
        private IDatabaseService Database;
        private ReplayMatchPlayer ReplayMatchPlayer;

        public PlayerNotesWindow(ReplayMatchPlayer player)
        {
            InitializeComponent();

            PlayerNotesWindowViewModel = (PlayerNotesWindowViewModel)DataContext;
            Database = PlayerNotesWindowViewModel.GetDatabaseService;
            ReplayMatchPlayer = player;

            LoadPlayerNotes();
        }

        private void LoadPlayerNotes()
        {
            var player = Database.ReplaysDb().HotsPlayer.ReadRecordFromPlayerId(ReplayMatchPlayer.PlayerId);

            PlayerNotesHeader.Text = player.BattleTagName;
            PlayerNotes.Text = player.Notes;
        }
    }
}
