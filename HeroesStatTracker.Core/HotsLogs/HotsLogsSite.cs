using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using static Heroes.Helpers.HeroesHelpers.Regions;

namespace HeroesMatchData.Core.HotsLogs
{
    public class HotsLogsSite : IHotsLogsService
    {
        public async Task<Uri> PlayerProfileAsync(Region region, string battleTag)
        {
            if (string.IsNullOrEmpty(battleTag))
                return null;

            string modifiedBattleTag = battleTag.Replace('#', '_');

            using (var httpClient = new HttpClient())
            {
                string json = await httpClient.GetStringAsync($"https://api.hotslogs.com/Public/Players/{(int)region}/{modifiedBattleTag}");

                if (string.IsNullOrWhiteSpace(json) || json == "null")
                    return null;

                dynamic jsonObject = JObject.Parse(json);
                long hotsLogsPlayerId = jsonObject.PlayerID; // get the playerId

                return new Uri($"https://www.hotslogs.com/Player/Profile?PlayerID={hotsLogsPlayerId}");
            }
        }
    }
}
