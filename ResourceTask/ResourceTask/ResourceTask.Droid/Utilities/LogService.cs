using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ResourceTask.Core.Utilities;

namespace ResourceTask.Droid.Utilities
{
    class LogService : ILogService
    {
        public void Exception(object sender, Exception ex)
        {
            throw new NotImplementedException();
        }

        public void Info(object sender, string message, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public void Warning(object sender, string warning, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        private void SavetoSd(string message)
        {
            var sdCardPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = System.IO.Path.Combine(sdCardPath, "iootext.txt");
            if (!System.IO.File.Exists(filePath))
            {
                using (System.IO.StreamWriter write = new System.IO.StreamWriter(filePath, true))
                {
                    write.Write(message);
                }
            }
        }
    }
}