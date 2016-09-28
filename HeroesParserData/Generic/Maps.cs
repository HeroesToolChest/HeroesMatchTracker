using System.Collections.Generic;

namespace HeroesParserData
{
    public static class Maps
    {
        public static List<string> GetMaps()
        {
            var maps = new List<string>();
            maps.Add("Battlefield of Eternity");
            maps.Add("Blackheart's Bay");
            maps.Add("Braxis Holdout");
            maps.Add("Cursed Hollow");
            maps.Add("Dragon Shire");
            maps.Add("Garden of Terror");
            maps.Add("Haunted Mines");
            maps.Add("Infernal Shrines");
            maps.Add("Sky Temple");
            maps.Add("Tomb of the Spider Queen");
            maps.Add("Towers of Doom");
            maps.Add("Warhead Junction");

            // Lost Cavern is not a map on rotation

            return maps;
        }
    }
}
