using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HeroesStatTracker.Data;
using MaterialDesignColors;
using System.Collections.Generic;

namespace HeroesStatTracker.Core.ViewModels.TitleBar
{
    public class PaletteSelectorWindowViewModel : ViewModelBase
    {
        private string _currentSelectedPrimary;
        private string _currentSelectedAccent;

        public IEnumerable<Swatch> Swatches { get; }

        public RelayCommand<bool> ToggleStyleCommand => new RelayCommand<bool>(x => StylePalette.ApplyStyle(x));
        public RelayCommand<bool> ToggleBaseCommand => new RelayCommand<bool>(x => StylePalette.ApplyBase(x));
        public RelayCommand<Swatch> ApplyPrimaryCommand => new RelayCommand<Swatch>(x => ApplyPrimary(x));
        public RelayCommand<Swatch> ApplyAccentCommand => new RelayCommand<Swatch>(x => ApplyAccent(x));
        public RelayCommand DefaultPaletteCommand => new RelayCommand(DefaultPalette);
        public string CurrentSelectedPrimary
        {
            get { return _currentSelectedPrimary; }
            set
            {
                _currentSelectedPrimary = value;
                RaisePropertyChanged();
            }
        }

        public string CurrentSelectedAccent
        {
            get { return _currentSelectedAccent; }
            set
            {
                _currentSelectedAccent = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAlternateStyle
        {
            get { return QueryDb.SettingsDb.UserSettings.IsAlternateStyle; }
            set
            {
                QueryDb.SettingsDb.UserSettings.IsAlternateStyle = value;
                RaisePropertyChanged();
            }
        }

        public bool IsNightMode
        {
            get { return QueryDb.SettingsDb.UserSettings.IsNightMode; }
            set
            {
                QueryDb.SettingsDb.UserSettings.IsNightMode = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PaletteSelectorWindowViewModel()
        {
            Swatches = new SwatchesProvider().Swatches;

            CurrentSelectedPrimary = QueryDb.SettingsDb.UserSettings.MainStylePrimary;
            CurrentSelectedAccent = QueryDb.SettingsDb.UserSettings.MainStyleAccent;
        }

        private void ApplyPrimary(Swatch swatch)
        {
            StylePalette.ApplyPrimary(swatch);
            CurrentSelectedPrimary = swatch.ToString();
            QueryDb.SettingsDb.UserSettings.MainStylePrimary = swatch.ToString();
        }

        private void ApplyAccent(Swatch swatch)
        {
            StylePalette.ApplyAccent(swatch);
            CurrentSelectedAccent = swatch.ToString();
            QueryDb.SettingsDb.UserSettings.MainStyleAccent = swatch.ToString();
        }

        private void DefaultPalette()
        {
            StylePalette.SetDefaultPalette();
        }
    }
}
