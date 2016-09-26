using System;

using Android.App;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content;

namespace ResourceTask.Droid
{
    [Application(Theme = "@android:style/Theme.Material")]
    public class Application : Android.App.Application
    {
        public Application(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }
        public override void OnCreate()
        {
    
            base.OnCreate();

        }
        public override void OnTerminate()
        {
            ApplicationContext.StopService(new Intent("com.xamarin.StopwatchService"));
            base.OnTerminate();
            
        }
       

    }
}