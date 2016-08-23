using System.Windows.Media.Imaging;

namespace HeroesParserData.Models.MatchModels
{
    public class MatchHeroBans : ObservableObject
    {
        private string _team0Ban0HeroName;
        private string _team0Ban1HeroName;
        private string _team1Ban0HeroName;
        private string _team1Ban1HeroName;
        private BitmapImage _team0Ban0;
        private BitmapImage _team0Ban1;
        private BitmapImage _team1Ban0;
        private BitmapImage _team1Ban1;

        public BitmapImage Team0Ban0
        {
            get { return _team0Ban0; }
            set
            {
                _team0Ban0 = value;
                RaisePropertyChangedEvent(nameof(Team0Ban0));
            }
        }
        public BitmapImage Team0Ban1
        {
            get { return _team0Ban1; }
            set
            {
                _team0Ban1 = value;
                RaisePropertyChangedEvent(nameof(Team0Ban1));
            }
        }
        public BitmapImage Team1Ban0
        {
            get { return _team1Ban0; }
            set
            {
                _team1Ban0 = value;
                RaisePropertyChangedEvent(nameof(Team1Ban0));
            }
        }
        public BitmapImage Team1Ban1
        {
            get { return _team1Ban1; }
            set
            {
                _team1Ban1 = value;
                RaisePropertyChangedEvent(nameof(Team1Ban1));
            }
        }

        public string Team0Ban0HeroName
        {
            get { return _team0Ban0HeroName; }
            set
            {
                _team0Ban0HeroName = value;
                RaisePropertyChangedEvent(nameof(Team0Ban0HeroName));
            }
        }
        public string Team0Ban1HeroName
        {
            get { return _team0Ban1HeroName; }
            set
            {
                _team0Ban1HeroName = value;
                RaisePropertyChangedEvent(nameof(Team0Ban1HeroName));
            }
        }
        public string Team1Ban0HeroName
        {
            get { return _team1Ban0HeroName; }
            set
            {
                _team1Ban0HeroName = value;
                RaisePropertyChangedEvent(nameof(Team1Ban0HeroName));
            }
        }
        public string Team1Ban1HeroName
        {
            get { return _team1Ban1HeroName; }
            set
            {
                _team1Ban1HeroName = value;
                RaisePropertyChangedEvent(nameof(Team1Ban1HeroName));
            }
        }
    }
}
