using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Heroes.Icons.Xml
{
    public interface IMatchAwards
    {
        BitmapImage GetMVPScreenAward(string mvpAwardType, MVPScreenColor mvpColor, out string awardName);
        BitmapImage GetMVPScoreScreenAward(string mvpAwardType, MVPScoreScreenColor mvpColor, out string awardName);
        string GetMatchAwardDescription(string mvpAwardType);
        List<string> GetMatchAwardsList();
    }
}
