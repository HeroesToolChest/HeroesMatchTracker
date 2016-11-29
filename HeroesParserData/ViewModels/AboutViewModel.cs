using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public string AppVersion
        {
            get { return HPDVersion.GetVersionAsString(); }
        }

        public string HeroesIconsVersion
        {
            get { return HPDVersion.GetHeroesIconsVersionAsString(); }
        }

        public string HeroesReplayParserVersion
        {
            get { return HPDVersion.GetHeroesReplayParserVersionAsString(); }
        }
    }
}
