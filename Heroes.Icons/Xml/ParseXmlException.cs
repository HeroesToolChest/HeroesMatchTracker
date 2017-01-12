using System;

namespace Heroes.Icons.Xml
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
