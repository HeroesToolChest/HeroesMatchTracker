using Heroes.ReplayParser;
using HeroesMatchTracker.Data.Models.Replays;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HeroesMatchTracker.Data.Generic
{
    public class ReplayHasher
    {
        public static string HashReplay(ReplayMatch replay, IEnumerable<Player> players)
        {
            StringBuilder sb = new StringBuilder(replay.RandomValue.ToString());
            sb.Append(replay.MapName);
            sb.Append(replay.ReplayBuild);
            sb.Append(replay.GameMode);

            foreach (Player player in players)
            {
                if (player == null || string.IsNullOrWhiteSpace(player.Character))
                    continue;

                sb.Append(player.Character);
            }

            using (SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));

                return string.Join(string.Empty, hash.Select(b => b.ToString("x2")).ToArray());
            }
        }

        public static string HashReplayOld(ReplayMatch replay)
        {
            string input = replay.RandomValue + replay.MapName + replay.ReplayBuild + replay.GameMode;
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));

                return string.Join(string.Empty, hash.Select(b => b.ToString("x2")).ToArray());
            }
        }
    }
}
