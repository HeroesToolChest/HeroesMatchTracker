using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace HeroesMatchData.Views.Matches
{
    /// <summary>
    /// Interaction logic for MatchListControl.xaml
    /// </summary>
    public partial class MatchListControl : UserControl
    {
        public MatchListControl()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }
    }
}
