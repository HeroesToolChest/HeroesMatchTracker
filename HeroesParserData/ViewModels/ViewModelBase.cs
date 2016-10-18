using HeroesIcons;
using NLog;
using System.Collections.Generic;

namespace HeroesParserData.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        protected HeroesInfo HeroesInfo {get; private set; }
        protected Logger ExceptionLog { get; private set; }
        protected Logger FailedReplaysLog { get; private set; }
        protected Logger SqlExceptionReplaysLog { get; private set; }
        protected List<string> AllSeasonsList { get; private set; }
        protected List<string> AllGameModesList { get; private set; }
        protected ViewModelBase()
        {
            HeroesInfo = App.HeroesInfo;
            ExceptionLog = LogManager.GetLogger("ExceptionLogFile");
            FailedReplaysLog = LogManager.GetLogger("UnParsedReplaysLogFile");
            SqlExceptionReplaysLog = LogManager.GetLogger("SqlExceptionReplaysLogFile");

            AllSeasonsList = Utilities.GetSeasonList();
            AllGameModesList = Utilities.GetGameModeList();
        }
    }
}