using System.Windows;
using System.Windows.Controls;

namespace HeroesParserData.Views
{
    /// <summary>
    /// Interaction logic for ReplaysControl.xaml
    /// </summary>
    public partial class ReplaysControl : UserControl
    {
        public ReplaysControl()
        {
            InitializeComponent();
        }

        private void LastestSaved_Click(object sender, RoutedEventArgs e)
        {
            if (!(bool)LastestSaved.IsChecked)
                LastestSaved.IsChecked = true;
        }

        private void LastSaved_Click(object sender, RoutedEventArgs e)
        {
            if (!(bool)LastSaved.IsChecked)
                LastSaved.IsChecked = true;
        }

        private void LastestUploaded_Click(object sender, RoutedEventArgs e)
        {
            if (!(bool)LastestUploaded.IsChecked)
                LastestUploaded.IsChecked = true;
        }

        private void LastUploaded_Click(object sender, RoutedEventArgs e)
        {
            if (!(bool)LastUploaded.IsChecked)
                LastUploaded.IsChecked = true;
        }
    }
}
