using System;
using static Heroes.ReplayParser.DataParser;

namespace HeroesParserData.Models
{
    public class ReplayFile : ObservableObject
    {
        private int? _build;
        private ReplayParseResult? _status;

        public string FileName { get; set; }
        public DateTime LastWriteTime { get; set; }   
        public string FilePath { get; set; }
        public int? Build
        {
            get { return _build; }
            set
            {
                _build = value;
                RaisePropertyChangedEvent(nameof(Build));
            }
        }
        public ReplayParseResult? Status
        {
            get { return _status;  }
            set
            {
                _status = value;
                RaisePropertyChangedEvent(nameof(Status));
            }
        }
    }
}
