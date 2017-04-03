using GalaSoft.MvvmLight.Ioc;
using HeroesMatchData.Core.ViewServices;
using System;
using System.Windows;

namespace HeroesMatchData.Views
{
    /// <summary>
    /// Interaction logic for LoadingOverlayWindow.xaml
    /// </summary>
    public partial class LoadingOverlayWindow : Window, ILoadingOverlayWindowService
    {
        public LoadingOverlayWindow()
        {
            InitializeComponent();

            SimpleIoc.Default.Register<ILoadingOverlayWindowService>(() => this);
        }

        public void CloseLoadingOverlay()
        {
            throw new NotImplementedException();
        }

        public async void ShowLoadingOverlay()
        {

        }
    }
}
