using System.Diagnostics;
using System.Windows.Input;

namespace HeroesParserData.Views
{
    /// <summary>
    /// Interaction logic for ReleaseNotesWindow.xaml
    /// </summary>
    public partial class ReleaseNotesWindow
    {
        public ReleaseNotesWindow()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(NavigationCommands.GoToPage, (sender, e) => Process.Start((string)e.Parameter)));
        }
    }
}
