using System;
using Android.App;
using Android.OS;
using Android.Widget;
using static Android.Manifest;

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
            if ((CheckSelfPermission(Permission.ReadExternalStorage) == (int)Android.Content.PM.Permission.Granted) &&
   (CheckSelfPermission(Permission.WriteExternalStorage) == (int)Android.Content.PM.Permission.Granted))
            {

            }
        }
    }
}