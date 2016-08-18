using System;

namespace HeroesIcons
{
    public class IconException : Exception
    {
        public IconException(string message, Exception ex)
            :base(message, ex)
        {

        }
    }
}
