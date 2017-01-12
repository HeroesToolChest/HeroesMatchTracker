namespace Heroes.Icons
{
    public class TalentTooltip
    {
        /// <summary>
        /// Gets the short tooltip of the talent
        /// </summary>
        public string Short { get; private set; }
        /// <summary>
        /// Gets the detailed tooltip of the talent
        /// </summary>
        public string Full { get; private set; }

        public TalentTooltip(string shortTooltip, string longTooltip)
        {
            Short = shortTooltip;
            Full = longTooltip;
        }

        public override string ToString()
        {
            return Short;
        }
    }
}
