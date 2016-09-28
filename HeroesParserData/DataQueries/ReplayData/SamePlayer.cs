using HeroesParserData.Models.DbModels;

namespace HeroesParserData.DataQueries.ReplayData
{
    public static partial class Query
    {
        internal static class SamePlayer
        {
            public static long CreateRecord(HeroesParserDataContext db, ReplaySamePlayer replaySamePlayer)
            {
                db.ReplaySamePlayers.Add(replaySamePlayer);
                db.SaveChanges();

                return replaySamePlayer.SamePlayerId;
            }
        }
    }
}
