using System;
using Android.App;
using Android.OS;
using Android.Widget;
using ResourceTask.Core.ViewModels;
using Android.Views;
using Android;
using Android.Support.V4.App;
using static Android.Manifest;
using Android.Support.Design.Widget;

namespace ResourceTask.Droid.Activities
{
    [Activity(Label = "Main", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button button;
        TextView text;
        ImageView image;
        MainViewModel viewModel;
        View layout;
        string timeFormat;

        static string[] PERMISSIONS_STORAGE = {
            Permission.ReadExternalStorage,
            Permission.WriteExternalStorage
        };
        static int[] ImagesSources =
        {
            Resource.Drawable.Clock,
            Resource.Drawable.Hourglass
        };
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            image = FindViewById<ImageView>(Resource.Id.imageView);
            button = FindViewById<Button>(Resource.Id.UpdateButton);
            text = FindViewById<TextView>(Resource.Id.CurrentTime);
            layout = FindViewById(Resource.Id.main_layout);
            timeFormat = GetString(Resource.String.TimeFormat);
            viewModel = TinyIoC.TinyIoCContainer.Current.Resolve<MainViewModel>();


            button.Click += delegate { UpdateButtonClick(); };
            UpdateButtonClick();
        }

        private void UpdateButtonClick()
        {
            var time = viewModel.GetTime(timeFormat);
            text.Text = GetString(Resource.String.CurrentTime, time);
            var tag = int.Parse( image.Tag.ToString());
            var partOfTheDay = (int)viewModel.GetPartOfTheDay;
            if(tag!= partOfTheDay)
            {
                image.SetImageResource(ImagesSources[partOfTheDay]);
            }
            if ((ActivityCompat.CheckSelfPermission(this, Permission.ReadExternalStorage) == (int)Android.Content.PM.Permission.Granted) &&
  (ActivityCompat.CheckSelfPermission(this, Permission.WriteExternalStorage) == (int)Android.Content.PM.Permission.Granted))
            {
                viewModel.LogToFile($"{Resource.String.ApplicationName} Current time:{time}");
            }
            else
            {
                RequestStoragePermission(() => UpdateButtonClick());
            }
        }

        private void RequestStoragePermission(Action callback)
        {
            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Permission.ReadExternalStorage)
                || ActivityCompat.ShouldShowRequestPermissionRationale(this, Permission.WriteExternalStorage))
            {

                // Provide an additional rationale to the user if the permission was not granted
                // and the user would benefit from additional context for the use of the permission.
                // For example, if the request has been denied previously.


                // Display a SnackBar with an explanation and a button to trigger the request.
                Snackbar.Make(layout, "Can I",
                    Snackbar.LengthIndefinite).SetAction("ok", new Action<View>(delegate (View obj)
                    {
                        ActivityCompat.RequestPermissions(this, PERMISSIONS_STORAGE, 1);
                    })).Show();
            }
            else
            {
                // Contact permissions have not been granted yet. Request them directly.
                ActivityCompat.RequestPermissions(this, PERMISSIONS_STORAGE, 1);
            }
        }
    }
}