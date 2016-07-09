using System;

namespace HeroesParserData
{
    public class SaveDataException : Exception
    {
        public SaveDataException(string message)
            :base(message) { }
    }
}
