using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace HeroesParserData.Models.HeroesModels
{
    public class HeroesUsable : ObservableObject
    {
        private ObservableCollection<bool> _isXOut;

        public BitmapImage[] HeroPortrait { get; set; } = new BitmapImage[15];
        public string[] HeroName { get; set; } = new string[15];
        public ObservableCollection<bool> IsXOut 
        {
            get { return _isXOut ?? (_isXOut = new ObservableCollection<bool>()); }
            set
            {
                _isXOut = value;
                RaisePropertyChangedEvent(nameof(IsXOut));
            }
        }
    }
}
