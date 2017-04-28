using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Heroes.Icons.Xml
{
    public interface IHomeScreens
    {
        List<Tuple<BitmapImage, Color>> GetListOfHomeScreens();
    }
}
