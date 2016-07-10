using Heroes.ReplayParser;
using System;
using static Heroes.ReplayParser.DataParser;

namespace HeroesParserData.Models
{
    public class ReplayFile : ObservableObject
    {
        private ReplayParseResult? _status;

        public string FileName { get; set; }
        public DateTime CreationTime { get; set; }   
             
        //public string HotsLogUploadStatus { get; set; }

        public string FilePath { get; set; }

        public ReplayParseResult? Status
        {
            get
            {
                return _status; 
            }
            set
            {
                _status = value;
                RaisePropertyChangedEvent("Status");
            }
        }
    }
}
