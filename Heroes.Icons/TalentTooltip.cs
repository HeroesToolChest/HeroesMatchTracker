namespace Heroes.Icons
{
    public class TalentTooltip
    {
        public TalentTooltip(string shortTooltip, string longTooltip)
        {
            Short = shortTooltip;
            Full = longTooltip;
        }

        /// <summary>
        /// Gets the short tooltip of the talent
        /// </summary>
        public string Short { get; private set; }

        /// <summary>
        /// Gets the detailed tooltip of the talent
        /// </summary>
        public string Full { get; private set; }

        public override string ToString()
        {
            return Short;
        }
    }
}
