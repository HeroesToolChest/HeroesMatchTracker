using System.Collections.Generic;

namespace Heroes.Helpers
{
    public static partial class HeroesHelpers
    {
        public static class Parties
        {
            public static List<string> GetPartyList()
            {
                return new List<string>
                {
                    "Solo",
                    "2 Stack",
                    "3 Stack",
                    "4 Stack",
                    "5 Stack",
                };
            }

            public static int GetPartyCount(string party)
            {
                if (party == "Solo")
                    return 1;
                else if (party == "2 Stack")
                    return 2;
                else if (party == "3 Stack")
                    return 3;
                else if (party == "4 Stack")
                    return 4;
                else if (party == "5 Stack")
                    return 5;
                else
                    return 0;
            }
        }
    }
}
