using Heroes.StormReplayParser;
using Heroes.StormReplayParser.Player;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HeroesMatchTracker.Shared
{
    public static class ReplayHasher
    {
        [SuppressMessage("Security", "CA5350:Do Not Use Weak Cryptographic Algorithms", Justification = "Used for unique id, not security")]
        public static string HashReplay(StormReplay replay)
        {
            if (replay is null)
                throw new System.ArgumentNullException(nameof(replay));

            StringBuilder sb = new StringBuilder(replay.RandomValue.ToString());
            sb.Append(replay.MapInfo.MapName);
            sb.Append(replay.ReplayBuild);
            sb.Append(replay.GameMode);

            foreach (StormPlayer player in replay.StormPlayersWithObservers)
            {
                if (player is null || string.IsNullOrWhiteSpace(player.PlayerHero?.HeroName))
                    continue;

                sb.Append(player.PlayerHero.HeroName);
            }

            using (SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));

                return string.Join(string.Empty, hash.Select(b => b.ToString("x2")).ToArray());
            }
        }
    }
}
