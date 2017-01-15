namespace HeroesStatTracker.Data.Queries
{
    public class QueriesBase
    {
        internal QueriesBase() { }

        protected bool LikeOperatorInputCheck(string operand, string input)
        {
            if (operand == "LIKE" && (input.Length == 1 || (input.Length >= 2 && input[0] != '%' && input[input.Length - 1] != '%')))
                return true;
            else
                return false;
        }
    }
}
