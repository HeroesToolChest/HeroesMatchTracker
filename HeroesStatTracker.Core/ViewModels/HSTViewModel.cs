using GalaSoft.MvvmLight;
using NLog;

namespace HeroesStatTracker.Core.ViewModels
{
    public class HstViewModel : ViewModelBase
    {
        protected HstViewModel()
        {
            ExceptionLog = LogManager.GetLogger(LogFileNames.Exceptions);
            WarningLog = LogManager.GetLogger(LogFileNames.WarningLogFileName);
            UnParsedReplaysLog = LogManager.GetLogger(LogFileNames.UnParsedReplaysLogFileName);
            TranslationsLog = LogManager.GetLogger(LogFileNames.TranslationLogFileName);
            HotsLogsLog = LogManager.GetLogger(LogFileNames.TranslationLogFileName);
        }

        protected Logger ExceptionLog { get; private set; }
        protected Logger WarningLog { get; private set; }
        protected Logger UnParsedReplaysLog { get; private set; }
        protected Logger TranslationsLog { get; private set; }
        protected Logger HotsLogsLog { get; private set; }
    }
}
