namespace HeroesMatchTracker.Core.User
{
    public interface ISelectedUserProfileService
    {
        string BattleTagName { get; }
        long PlayerId { get; }
        int RegionId { get; }

        void SetProfile(string battleTag, int region);
    }
}
