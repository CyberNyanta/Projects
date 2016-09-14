using ResourceTask.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTask.Core.ViewModels
{
    public class BaseViewModel: INotifyPropertyChanged
    {
        protected ILogService _logService;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void LogToFile(string message)
        {
            _logService.Info(null, message);
        }
    }
}
