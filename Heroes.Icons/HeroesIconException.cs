using System;

namespace Heroes.Icons
{
    [Serializable]
    public class HeroesIconException : Exception
    {
        public HeroesIconException(string message, Exception ex)
            :base(message, ex)
        {

        }

        public HeroesIconException(string message)
            : base(message)
        {

        }
    }
}
