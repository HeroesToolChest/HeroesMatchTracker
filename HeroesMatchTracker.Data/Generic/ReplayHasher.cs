using HeroesMatchTracker.Data.Models.Replays;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HeroesMatchTracker.Data.Generic
{
    public class ReplayHasher
    {
        public static string HashReplay(ReplayMatch replay)
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
