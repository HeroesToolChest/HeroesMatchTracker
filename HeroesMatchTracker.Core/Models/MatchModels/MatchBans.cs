using GalaSoft.MvvmLight;
using System.IO;

namespace HeroesMatchTracker.Core.Models.MatchModels
{
    public class MatchBans : ObservableObject
    {
        private string _team0Ban0HeroName;
        private string _team0Ban1HeroName;
        private string _team1Ban0HeroName;
        private string _team1Ban1HeroName;
        private Stream _team0Ban0;
        private Stream _team0Ban1;
        private Stream _team1Ban0;
        private Stream _team1Ban1;

        public Stream Team0Ban0
        {
            get { return _team0Ban0; }
            set
            {
                _team0Ban0 = value;
                RaisePropertyChanged();
            }
        }

        public Stream Team0Ban1
        {
            get { return _team0Ban1; }
            set
            {
                _team0Ban1 = value;
                RaisePropertyChanged();
            }
        }

        public Stream Team1Ban0
        {
            get { return _team1Ban0; }
            set
            {
                _team1Ban0 = value;
                RaisePropertyChanged();
            }
        }

        public Stream Team1Ban1
        {
            get { return _team1Ban1; }
            set
            {
                _team1Ban1 = value;
                RaisePropertyChanged();
            }
        }

        public string Team0Ban0HeroName
        {
            get { return _team0Ban0HeroName; }
            set
            {
                _team0Ban0HeroName = value;
                RaisePropertyChanged();
            }
        }

        public string Team0Ban1HeroName
        {
            get { return _team0Ban1HeroName; }
            set
            {
                _team0Ban1HeroName = value;
                RaisePropertyChanged();
            }
        }

        public string Team1Ban0HeroName
        {
            get { return _team1Ban0HeroName; }
            set
            {
                _team1Ban0HeroName = value;
                RaisePropertyChanged();
            }
        }

        public string Team1Ban1HeroName
        {
            get { return _team1Ban1HeroName; }
            set
            {
                _team1Ban1HeroName = value;
                RaisePropertyChanged();
            }
        }
    }
}
