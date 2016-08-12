using System;

namespace HeroesParserData
{
    [Serializable]
    public class SaveDataException : Exception
    {
        public SaveDataException(string message)
            :base(message) { }
    }
}
