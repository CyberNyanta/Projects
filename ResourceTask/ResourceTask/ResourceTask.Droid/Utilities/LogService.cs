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
using Java.Interop;
using static Android.Manifest;

namespace ResourceTask.Droid.Utilities
{
    class LogService : ILogService
    {
        public void Exception(object sender, Exception ex)
        {
            throw new NotImplementedException();
        }

        public void Info(object sender, string message)
        {
            Save(message);
        }

        public void Warning(object sender, string warning)
        {
            throw new NotImplementedException();
        }

        private void Save(string message)
        {
            //var sdCardPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.Path;
            var filePath = System.IO.Path.Combine(sdCardPath, "LogFile.txt");
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