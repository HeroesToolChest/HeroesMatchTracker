using System;

namespace HeroesMatchTracker.Data.Queries.Replays
{
    [Serializable]
    public class TranslationException : Exception
    {
        public TranslationException(string message)
            : base(message)
        { }
        public TranslationException(string message, Exception ex)
            : base(message, ex)
        { }
    }
}
