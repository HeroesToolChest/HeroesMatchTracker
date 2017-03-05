namespace HeroesStatTracker.Core.Messaging
{
    public class MatchesDataMessage
    {
        public MatchesTab MatchTab { get; set; }
        public string SelectedCharacter { get; set; }
        public string SelectedBattleTagName { get; set; }
    }
}
