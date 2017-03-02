namespace HeroesStatTracker.Core.Models
{
    public class MainPageItem
    {
        public MainPageItem(MainPage page)
        {
            if (page == MainPage.Home)
                Name = "Home";
            else if (page == MainPage.Matches)
                Name = "Matches";
            else if (page == MainPage.ReplayParser)
                Name = "Replay Parser";
            else if (page == MainPage.RawData)
                Name = "Raw Data Tables";

            MainPage = page;
        }

        public string Name { get; }
        public MainPage MainPage { get; }
    }
}
