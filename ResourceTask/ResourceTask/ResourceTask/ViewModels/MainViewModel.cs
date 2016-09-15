using System;
using ResourceTask.Core.Utilities;
using System.ComponentModel;
using ResourceTask.Core.Enums;
using System.Globalization;


namespace ResourceTask.Core.ViewModels
{
    public class MainViewModel
    {
        protected ILogService _logService;

        public MainViewModel(ILogService logService)
        {
            _logService = logService;
        }        

        public void LogToFile(string message)
        {
            _logService.Info(null, message);
        }

        public PartsOfTheDay GetPartOfTheDay
        {
            get { return DateTime.Now.ToString("tt", CultureInfo.InvariantCulture).Equals("AM") ? PartsOfTheDay.AM : PartsOfTheDay.PM; }
        }
        public string GetTime(string regex)
        {
            return DateTime.Now.ToString(regex);
        }

    }
}
