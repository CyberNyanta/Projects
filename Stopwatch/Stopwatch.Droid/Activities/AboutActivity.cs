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
using Android.Graphics;
using Stopwatch.Droid.Widgets;

namespace Stopwatch.Droid.Activities
{
    [Activity(Label = "AboutActivity")]
    public class AboutActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AboutLayout);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetDisplayShowHomeEnabled(true);
            
            

            LinearLayout imageContainer = FindViewById<LinearLayout>(Resource.Id.image_container);
            for (int i = 0; i < 4; i++)
            {
                ImageView image = new ImageView(this);
                var coeff = Resources.DisplayMetrics.Density;
                image.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                image.Background = GetDrawable(Resource.Drawable.img);

                imageContainer.AddView(image);
            }
            


            // Create your application here

        }



        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    OnBackPressed();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}