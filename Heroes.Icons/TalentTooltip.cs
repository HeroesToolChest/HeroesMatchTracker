using System;

namespace Heroes.Icons
{
    public class TalentTooltip
    {
        public TalentTooltip() { }

        public TalentTooltip(string shortTooltip, string longTooltip, int? mana, bool isPerManaCost, int? cooldown, bool isChargeCooldown, HeroMana manaType)
        {
            Short = shortTooltip;
            Full = longTooltip;
            Mana = mana;
            IsPerManaCost = isPerManaCost;
            Cooldown = cooldown;
            IsChargeCooldown = isChargeCooldown;
            ManaType = manaType;
        }

        /// <summary>
        /// Gets the short tooltip of the talent
        /// </summary>
        public string Short { get; }

        /// <summary>
        /// Gets the detailed tooltip of the talent
        /// </summary>
        public string Full { get; }

        /// <summary>
        /// Get the cooldown of the talent
        /// </summary>
        public int? Cooldown { get; }

        /// <summary>
        /// Get the mana (also brew, fury, etc...) cost of the talent
        /// </summary>
        public int? Mana { get; }

        /// <summary>
        /// Is the mana cost time based
        /// </summary>
        public bool IsPerManaCost { get; }

        /// <summary>
        /// Is the cooldown a charge cooldown
        /// </summary>
        public bool IsChargeCooldown { get; }

        /// <summary>
        /// Type of energy: Mana is default
        /// </summary>
        public HeroMana ManaType { get; }

        /// <summary>
        /// Returns a string of the talent's cooldown and mana cost
        /// </summary>
        /// <returns></returns>
        public string GetTalentSubInfo()
        {
            string text = string.Empty;

            if (Mana.HasValue)
            {
                if (IsPerManaCost)
                    text += $"{ManaType.ToString()}: {Mana.Value} per second";
                else
                    text += $"{ManaType.ToString()}: {Mana.Value}";
            }

            if (Mana.HasValue && Cooldown.HasValue)
                text += Environment.NewLine;

            if (Cooldown.HasValue)
            {
                string time = Cooldown.Value > 1 ? "seconds" : "second";

                if (IsChargeCooldown)
                    text += $"Charge Cooldown: {Cooldown.Value} {time}";
                else
                    text += $"Cooldown: {Cooldown.Value} {time}";
            }

            return text;
        }

        public override string ToString()
        {
            return Short;
        }
    }
}
