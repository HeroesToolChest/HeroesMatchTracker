using System;
using System.Collections.Generic;
using System.Drawing;

namespace Heroes.Icons.Xml
{
    public interface IHomeScreens
    {
        List<Tuple<Uri, Color>> GetListOfHomeScreens();
    }
}
