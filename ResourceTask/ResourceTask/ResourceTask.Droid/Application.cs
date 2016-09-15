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
using System.ComponentModel.Design;
using TinyIoC;
using ResourceTask.Core.Utilities;
using ResourceTask.Droid.Utilities;

namespace ResourceTask.Droid
{
    [Application(Theme = "@android:style/Theme.Holo.Light")]
    public class Application : Android.App.Application
    {
        public Application(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }
        public override void OnCreate()
        {
           			
            var container = TinyIoCContainer.Current;

            container.Register<ILogService, LogService>();


            base.OnCreate();
            	
        }
    }
}