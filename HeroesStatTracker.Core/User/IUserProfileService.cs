namespace HeroesMatchData.Core.User
{
    public interface IUserProfileService
    {
        string BattleTagName { get; }
        long PlayerId { get; }
        int RegionId { get; }

        void SetProfile(string battleTag, int region);
    }
}
