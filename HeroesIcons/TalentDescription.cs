namespace HeroesIcons
{
    public class TalentDescription
    {
        /// <summary>
        /// Gets the short description of the talent
        /// </summary>
        public string Short { get; private set; }
        /// <summary>
        /// Gets the detailed desciption of the talent
        /// </summary>
        public string Full { get; private set; }

        public TalentDescription(string shortDescription, string longDescription)
        {
            Short = shortDescription;
            Full = longDescription;
        }

        public override string ToString()
        {
            return Short;
        }
    }
}
