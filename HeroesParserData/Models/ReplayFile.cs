using Heroes.ReplayParser;
using System;

namespace HeroesParserData.Models
{
    public class ReplayFile : ObservableObject
    {
        private ReplayParseStatus? _status;

        public string FileName { get; set; }
        public DateTime CreationTime { get; set; }   
             
        //public string HotsLogUploadStatus { get; set; }
        public string FilePath { get; set; }

        public ReplayParseStatus? Status
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
