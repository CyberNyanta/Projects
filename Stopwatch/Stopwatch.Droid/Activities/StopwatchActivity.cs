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
    [Activity(Label = "StopwatchActivity", Icon = "@drawable/icon")]
    public class StopwatchActivity : Activity
    {


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Stopwatch);
        }



    }
}