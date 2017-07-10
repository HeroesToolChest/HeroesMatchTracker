using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Heroes.Icons.Xml
{
    public interface IMatchAwards
    {
        /// <summary>
        /// Returns the MVPScreen award BitmapImage of the given mvpAwardType and color
        /// </summary>
        /// <param name="mvpAwardType">Reference name of award</param>
        /// <param name="mvpColor">Color of icon</param>
        /// <param name="awardName"></param>
        /// <returns></returns>
        BitmapImage GetMVPScreenAward(string mvpAwardType, MVPScreenColor mvpColor, out string awardName);

        /// <summary>
        /// Returns the ScoreScreen award BitmapImage of the given mvpAwardType and color
        /// </summary>
        /// <param name="mvpAwardType">Reference name of award</param>
        /// <param name="mvpColor">Color of icon</param>
        /// <param name="awardName"></param>
        /// <returns></returns>
        BitmapImage GetMVPScoreScreenAward(string mvpAwardType, MVPScoreScreenColor mvpColor, out string awardName);

        /// <summary>
        /// Returns the description of the award
        /// </summary>
        /// <param name="mvpAwardType">Reference name of award</param>
        /// <returns></returns>
        string GetMatchAwardDescription(string mvpAwardType);

        /// <summary>
        /// Returns a list of all the match awards (reference names)
        /// </summary>
        /// <returns></returns>
        List<string> GetMatchAwardsList();

        int TotalCountOfAwards();
    }
}
