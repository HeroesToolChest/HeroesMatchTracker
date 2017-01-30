namespace HeroesStatTracker.Core.User
{
    public interface IUserProfileService
    {
        string BattleTagName { get; set; }
        long PlayerId { get; set; }
        int RegionId { get; set; }

        void SetProfile();
    }
}
