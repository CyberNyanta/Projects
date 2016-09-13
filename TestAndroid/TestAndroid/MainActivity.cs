using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace TestAndroid.Droid
{
    [Activity(Label = "TestAndroid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.UpdateButton);
            TextView text = FindViewById<TextView>(Resource.Id.CurrentTime);
            button.Click += delegate { text.Text = GetString(Resource.String.CurrentTime, DateTime.Now.ToString(Resource.String.TimeFormat.ToString())); };

        
        }

       
    }
}

