using System;

using Android.App;
using Android.Runtime;
using Android.Views;
using Android.Widget;


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
    }
}