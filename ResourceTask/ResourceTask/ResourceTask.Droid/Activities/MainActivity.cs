using System;
using Android.App;
using Android.OS;
using Android.Widget;

namespace ResourceTask.Droid.Activities
{
    [Activity(Label = "Main", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            Button button = FindViewById<Button>(Resource.Id.UpdateButton);
            TextView text = FindViewById<TextView>(Resource.Id.CurrentTime);
            text.Text = GetString(Resource.String.CurrentTime, DateTime.Now.ToString(GetString(Resource.String.TimeFormat)));
            button.Click += delegate { text.Text = GetString(Resource.String.CurrentTime, DateTime.Now.ToString(GetString(Resource.String.TimeFormat))); };
        }
    }
}