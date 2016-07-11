using NLog;

namespace HeroesParserData.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        protected Logger ExceptionLog { get; private set; }
        protected Logger FailedReplaysLog { get; private set; }
        protected Logger SqlExceptionReplaysLog { get; private set; }

        protected ViewModelBase()
        {
            ExceptionLog = LogManager.GetLogger("ExceptionLogFile");
            FailedReplaysLog = LogManager.GetLogger("UnParsedReplaysLogFile");
            SqlExceptionReplaysLog = LogManager.GetLogger("SqlExceptionReplaysLogFile");
        }
    }
}