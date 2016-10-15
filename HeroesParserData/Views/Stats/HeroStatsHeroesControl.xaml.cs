using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HeroesParserData.Views.Stats
{
    /// <summary>
    /// Interaction logic for HeroStatsHeroesControl.xaml
    /// </summary>
    public partial class HeroStatsHeroesControl : UserControl
    {
        public HeroStatsHeroesControl()
        {
            InitializeComponent();
        }

        private void QuickMatchExpander_Expanded(object sender, RoutedEventArgs e)
        {
            //UnrankedDraftExpander.IsExpanded = true;
        }

        private void UnrankedDraftExpander_Expanded(object sender, RoutedEventArgs e)
        {
            //QuickMatchExpander.IsExpanded = true;
        }

        private void HeroLeagueExpander_Expanded(object sender, RoutedEventArgs e)
        {
            //TeamLeagueExpander.IsExpanded = true;
        }

        private void TeamLeagueExpander_Expanded(object sender, RoutedEventArgs e)
        {
            //HeroLeagueExpander.IsExpanded = true;
        }

        private void QuickMatchExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            //UnrankedDraftExpander.IsExpanded = false;
        }

        private void UnrankedDraftExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            //QuickMatchExpander.IsExpanded = false;
        }

        private void HeroLeagueExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            //TeamLeagueExpander.IsExpanded = false;
        }

        private void TeamLeagueExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            //HeroLeagueExpander.IsExpanded = false;
        }
    }
}
