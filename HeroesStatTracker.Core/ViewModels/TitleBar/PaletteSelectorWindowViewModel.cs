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

        private IDatabaseService Database;

        /// <summary>
        /// Constructor
        /// </summary>
        public PaletteSelectorWindowViewModel(IDatabaseService database)
        {
            Database = database;
            Swatches = new SwatchesProvider().Swatches;

            CurrentSelectedPrimary = Database.SettingsDb().UserSettings.MainStylePrimary;
            CurrentSelectedAccent = Database.SettingsDb().UserSettings.MainStyleAccent;
        }

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
            get { return Database.SettingsDb().UserSettings.IsAlternateStyle; }
            set
            {
                Database.SettingsDb().UserSettings.IsAlternateStyle = value;
                RaisePropertyChanged();
            }
        }

        public bool IsNightMode
        {
            get { return Database.SettingsDb().UserSettings.IsNightMode; }
            set
            {
                Database.SettingsDb().UserSettings.IsNightMode = value;
                RaisePropertyChanged();
            }
        }

        private void ApplyPrimary(Swatch swatch)
        {
            StylePalette.ApplyPrimary(swatch);
            CurrentSelectedPrimary = swatch.ToString();
            Database.SettingsDb().UserSettings.MainStylePrimary = swatch.ToString();
        }

        private void ApplyAccent(Swatch swatch)
        {
            StylePalette.ApplyAccent(swatch);
            CurrentSelectedAccent = swatch.ToString();
            Database.SettingsDb().UserSettings.MainStyleAccent = swatch.ToString();
        }

        private void DefaultPalette()
        {
            StylePalette.SetDefaultPalette(Database);
        }
    }
}
