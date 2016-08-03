using HeroesIcons.Heroes;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace HeroesIcons
{
    public class TalentIcons
    {
        private Dictionary<string, Uri> Talents = new Dictionary<string, Uri>();

        public TalentIcons()
        {
            SetTalentNamesIcons();
        }

        public BitmapImage GetTalentIcon(string nameOfTalent)
        {
            Uri uri;
            if (string.IsNullOrEmpty(nameOfTalent))
                return null;

            if (!Talents.TryGetValue(nameOfTalent, out uri))
                uri = Talents[nameof(_Generic.IconDefault)];

            return new BitmapImage(uri);
        }

        private Uri SetHeroTalentUri(string hero, string fileName)
        {
            return new Uri($@"/HeroesIcons;component//Icons/Talents/{hero}/{fileName}", UriKind.Relative);
        }

        private void SetTalentNamesIcons()
        {
            Talents.Add(nameof(_Generic.IconDefault), SetHeroTalentUri(_Generic.Hero, _Generic.IconDefault));
            Talents.Add(nameof(Abathur.AbathurCombatStyleSurvivalInstincts), SetHeroTalentUri(Abathur.Hero, Abathur.AbathurCombatStyleSurvivalInstincts));
        }
    }
}
