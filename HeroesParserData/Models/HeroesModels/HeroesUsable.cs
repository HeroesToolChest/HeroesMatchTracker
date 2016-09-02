using System.Windows.Media.Imaging;

namespace HeroesParserData.Models.HeroesModels
{
    public class HeroesUsable : ObservableObject
    {
        private bool _isXOut1;
        private bool _isXOut2;
        private bool _isXOut3;
        private bool _isXOut4;
        private bool _isXOut5;
        private bool _isXOut6;
        private bool _isXOut7;
        private bool _isXOut8;
        private bool _isXOut9;
        private bool _isXOut10;
        private bool _isXOut11;
        private bool _isXOut12;
        private bool _isXOut13;
        private bool _isXOut14;
        private bool _isXOut15;

        public BitmapImage HeroPortrait1 { get; set; }
        public string HeroName1 { get; set; }
        public bool IsXOut1
        {
            get { return _isXOut1; }
            set
            {
                _isXOut1 = value;
                RaisePropertyChangedEvent(nameof(IsXOut1));
            }
        }
        public BitmapImage HeroPortrait2 { get; set; }
        public string HeroName2 { get; set; }
        public bool IsXOut2
        {
            get { return _isXOut2; }
            set
            {
                _isXOut2 = value;
                RaisePropertyChangedEvent(nameof(IsXOut2));
            }
        }
        public BitmapImage HeroPortrait3 { get; set; }
        public string HeroName3 { get; set; }
        public bool IsXOut3
        {
            get { return _isXOut3; }
            set
            {
                _isXOut3 = value;
                RaisePropertyChangedEvent(nameof(IsXOut3));
            }
        }
        public BitmapImage HeroPortrait4 { get; set; }
        public string HeroName4 { get; set; }
        public bool IsXOut4
        {
            get { return _isXOut4; }
            set
            {
                _isXOut4 = value;
                RaisePropertyChangedEvent(nameof(IsXOut4));
            }
        }
        public BitmapImage HeroPortrait5 { get; set; }
        public string HeroName5 { get; set; }
        public bool IsXOut5
        {
            get { return _isXOut5; }
            set
            {
                _isXOut5 = value;
                RaisePropertyChangedEvent(nameof(IsXOut5));
            }
        }
        public BitmapImage HeroPortrait6 { get; set; }
        public string HeroName6 { get; set; }
        public bool IsXOut6
        {
            get { return _isXOut6; }
            set
            {
                _isXOut6 = value;
                RaisePropertyChangedEvent(nameof(IsXOut6));
            }
        }
        public BitmapImage HeroPortrait7 { get; set; }
        public string HeroName7 { get; set; }
        public bool IsXOut7
        {
            get { return _isXOut7; }
            set
            {
                _isXOut7 = value;
                RaisePropertyChangedEvent(nameof(IsXOut7));
            }
        }
        public BitmapImage HeroPortrait8 { get; set; }
        public string HeroName8 { get; set; }
        public bool IsXOut8
        {
            get { return _isXOut8; }
            set
            {
                _isXOut8 = value;
                RaisePropertyChangedEvent(nameof(IsXOut8));
            }
        }
        public BitmapImage HeroPortrait9 { get; set; }
        public string HeroName9 { get; set; }
        public bool IsXOut9
        {
            get { return _isXOut9; }
            set
            {
                _isXOut9 = value;
                RaisePropertyChangedEvent(nameof(IsXOut9));
            }
        }
        public BitmapImage HeroPortrait10 { get; set; }
        public string HeroName10 { get; set; }
        public bool IsXOut10
        {
            get { return _isXOut10; }
            set
            {
                _isXOut10 = value;
                RaisePropertyChangedEvent(nameof(IsXOut10));
            }
        }
        public BitmapImage HeroPortrait11 { get; set; }
        public string HeroName11 { get; set; }
        public bool IsXOut11
        {
            get { return _isXOut11; }
            set
            {
                _isXOut11 = value;
                RaisePropertyChangedEvent(nameof(IsXOut11));
            }
        }
        public BitmapImage HeroPortrait12 { get; set; }
        public string HeroName12 { get; set; }
        public bool IsXOut12
        {
            get { return _isXOut12; }
            set
            {
                _isXOut12 = value;
                RaisePropertyChangedEvent(nameof(IsXOut12));
            }
        }
        public BitmapImage HeroPortrait13 { get; set; }
        public string HeroName13 { get; set; }
        public bool IsXOut13
        {
            get { return _isXOut13; }
            set
            {
                _isXOut13 = value;
                RaisePropertyChangedEvent(nameof(IsXOut13));
            }
        }
        public BitmapImage HeroPortrait14 { get; set; }
        public string HeroName14 { get; set; }
        public bool IsXOut14
        {
            get { return _isXOut14; }
            set
            {
                _isXOut14 = value;
                RaisePropertyChangedEvent(nameof(IsXOut14));
            }
        }
        public BitmapImage HeroPortrait15 { get; set; }
        public string HeroName15 { get; set; }
        public bool IsXOut15
        {
            get { return _isXOut15; }
            set
            {
                _isXOut15 = value;
                RaisePropertyChangedEvent(nameof(IsXOut15));
            }
        }
    }
}
