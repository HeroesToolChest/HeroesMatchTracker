using System;

namespace HeroesIcons
{
    [Serializable]
    public class ParseXmlException : Exception
    {
        public ParseXmlException(string message, Exception ex)
            :base(message, ex)
        {

        }

        public ParseXmlException(string message)
            : base(message)
        {

        }
    }
}
