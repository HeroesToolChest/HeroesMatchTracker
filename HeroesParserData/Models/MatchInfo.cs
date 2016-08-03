using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace HeroesParserData.Models
{
    public class MatchInfo
    {
        public string PlayerName { get; set; }
        public string CharacterName { get; set; }
        public int CharacterLevel { get; set; }
        public BitmapImage Talent1 { get; set; }
        public BitmapImage Talent4 { get; set; }
        public BitmapImage Talent7 { get; set; }
        public BitmapImage Talent10 { get; set; }
        public BitmapImage Talent13 { get; set; }
        public BitmapImage Talent16 { get; set; }
        public BitmapImage Talent20 { get; set; }
    }
}
