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
using Stopwatch.Core.Utils;

namespace Stopwatch.Droid.Activities
{
    [Activity(Label = "LoginActivity",MainLauncher = true)]
    public class LoginActivity : Activity
    {
        EditText login;
        EditText password;
        Button button;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            login = FindViewById<EditText>(Resource.Id.input_login);
            password = FindViewById<EditText>(Resource.Id.input_password);
            button = FindViewById<Button>(Resource.Id.submit_button);
            button.Click += delegate { SubmitClick(); };
            
        }

        private void SubmitClick()
        {
            if (ValidationUtils.LoginValidation(login.Text))
            {
                var activity = new Intent(this, typeof(StopwatchActivity));
                activity.PutExtra("login", login.Text);
                StartActivity(activity);
            }
            else
            {
                login.Error = GetString(Resource.String.LoginError);
            }
        }

    }
}