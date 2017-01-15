using GalaSoft.MvvmLight;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesStatTracker.Core.ViewModels
{
    public class HSTViewModel : ViewModelBase
    {
        protected Logger ExceptionLog { get; private set; }
        //protected Logger FailedReplaysLog { get; private set; }
        //protected Logger SqlExceptionReplaysLog { get; private set; }
        protected Logger WarningLog { get; private set; }
        protected Logger HotsLogsLog { get; private set; }
        protected Logger UnParsedReplaysLog { get; private set; }

        protected HSTViewModel()
        {
            ExceptionLog = LogManager.GetLogger(LogFileNames.Exceptions);
            //FailedReplaysLog = LogManager.GetLogger(LogFile.UnParsedReplaysLogFile);
            //SqlExceptionReplaysLog = LogManager.GetLogger(LogFile.SqlExceptionReplaysLogFile);
            WarningLog = LogManager.GetLogger(LogFileNames.WarningLogFileName);
            UnParsedReplaysLog = LogManager.GetLogger(LogFileNames.UnParsedReplaysLogFileName);
            //HotsLogsLog = LogManager.GetLogger(LogFileNames.);
        }
    }
}
