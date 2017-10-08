using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Heroes.Icons.Xml
{
    public interface IHomeScreens
    {
        Stream GetHomescreen(string homescreenName);

        /// <summary>
        /// Returns the font glow color assoicated with the homescreen, return black if homescreen not found
        /// </summary>
        /// <param name="homescreenName"></param>
        /// <returns></returns>
        Color GetHomescreenFontGlowColor(string homescreenName);

        /// <summary>
        /// Returns a list all all homescreen names
        /// </summary>
        /// <returns></returns>
        List<string> GetHomescreensList();
    }
}
