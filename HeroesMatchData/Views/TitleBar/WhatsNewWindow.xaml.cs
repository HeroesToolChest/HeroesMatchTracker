using HeroesMatchData.Core;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace HeroesMatchData.Views.TitleBar
{
    /// <summary>
    /// Interaction logic for WhatsNewWindow.xaml
    /// </summary>
    public partial class WhatsNewWindow
    {
        public WhatsNewWindow()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(NavigationCommands.GoToPage, (sender, e) =>
            {
                if (ExternalLinkedSites.IsApprovedSite(new Uri(e.Parameter.ToString())))
                {
                    Process.Start((string)e.Parameter);
                }
            }));
        }
    }
}
