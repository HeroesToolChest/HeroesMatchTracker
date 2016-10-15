using HeroesIcons;
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

        public static MapName GetEnumMapName(string mapName)
        {
            switch (mapName)
            {
                case "Battlefield of Eternity":
                    return MapName.BattlefieldofEternity;
                case "Blackheart's Bay":
                    return MapName.BlackheartsBay;
                case "Braxis Holdout":
                    return MapName.BraxisHoldout;
                case "Cursed Hollow":
                    return MapName.CursedHollow;
                case "Dragon Shire":
                    return MapName.DragonShire;
                case "Garden of Terror":
                    return MapName.GardenofTerror;
                case "Haunted Mines":
                    return MapName.HauntedMines;
                case "Infernal Shrines":
                    return MapName.InfernalShrines;
                case "Lost Cavern":
                    return MapName.LostCavern;
                case "Sky Temple":
                    return MapName.SkyTemple;
                case "Tomb of the Spider Queen":
                    return MapName.TomboftheSpiderQueen;
                case "Towers of Doom":
                    return MapName.TowersofDoom;
                case "Warhead Junction":
                    return MapName.WarheadJunction;
                default:
                    return MapName.Unknown;
            }
        }
    }
}
