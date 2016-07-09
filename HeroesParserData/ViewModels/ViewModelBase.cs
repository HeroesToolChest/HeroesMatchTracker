using NLog;

namespace HeroesParserData.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        protected Logger Logger { get; private set; }

        protected ViewModelBase()
        {
            Logger = LogManager.GetLogger(GetType().FullName);
        }
    }
}