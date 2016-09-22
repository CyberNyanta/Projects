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
using Android.Animation;
using Android.Graphics.Drawables;
using Android.Util;

namespace Stopwatch.Droid.Activities
{
    [Activity(Label = "StopwatchActivity", Icon = "@drawable/icon")]
    public class StopwatchActivity : Activity
    {
        #region Listeners
        class ClickListener : Java.Lang.Object, View.IOnClickListener
        {
            StopwatchActivity instance;
            public ClickListener(StopwatchActivity instance)
            {
                this.instance = instance;
            }

            public void OnClick(View v)
            {
                instance.expanded = !(instance.expanded);
                if (instance.expanded)
                {
                    instance.expandFab();
                }
                else
                {
                    instance.collapseFab();
                }
            }
        }

        class PreDrawListener : Java.Lang.Object, ViewTreeObserver.IOnPreDrawListener
        {

            StopwatchActivity instance;
            public PreDrawListener(StopwatchActivity instance)
            {
                this.instance = instance;
            }
            public bool OnPreDraw()
            {

                instance.fabContainer.ViewTreeObserver.RemoveOnPreDrawListener(this);
                instance.offset1 = instance.fab.GetY() - instance.fabAction1.GetY();
                instance.fabAction1.TranslationY = instance.offset1;
                instance.offset2 = instance.fab.GetY() - instance.fabAction2.GetY();
                instance.fabAction2.TranslationY = instance.offset2;
                instance.offset3 = instance.fab.GetY() - instance.fabAction3.GetY();
                instance.fabAction3.TranslationY = instance.offset3;
                return true;
            }
        }
        #endregion

        

        #region prop
        const String TAG = "Floating Action Button";
        const String TRANSLATION_Y = "translationY";
        internal ImageButton fab;
        internal bool expanded;
        internal View fabAction1;
        internal View fabAction2;
        internal View fabAction3;
        internal float offset1;
        internal float offset2;
        internal float offset3;
        LinearLayout startlayout;
        ImageButton startButton;
        Chronometer chronometer;

        internal ViewGroup fabContainer;
        long startTime;
        #endregion


        protected override void OnCreate(Bundle instance)
        {
            base.OnCreate(instance);
            SetContentView(Resource.Layout.Stopwatch);
           
            fabContainer = (ViewGroup)FindViewById(Resource.Id.fab_container);
            fab = (ImageButton)FindViewById(Resource.Id.fab);
            fabAction1 = FindViewById(Resource.Id.fab_action_1);
            fabAction2 = FindViewById(Resource.Id.fab_action_2);
            fabAction3 = FindViewById(Resource.Id.fab_action_3);
            chronometer = FindViewById<Chronometer>(Resource.Id.chronometer);

            startlayout = FindViewById<LinearLayout>(Resource.Id.start_layout);
            var userName = FindViewById<TextView>(Resource.Id.user_name);
            userName.Text = Intent.GetStringExtra("login");
            startButton = FindViewById<ImageButton>(Resource.Id.start_button);

            fab.SetOnClickListener(new ClickListener(this));

            fabContainer.ViewTreeObserver.AddOnPreDrawListener(new PreDrawListener(this));

            startButton.Click += delegate
            {
                StartButtonClick();
            };
            fabAction1.Click += delegate
            {
                FabAction1();
            };
            fabAction2.Click += delegate
            {
                FabAction2();
            };
            fabAction3.Click += delegate
            {
                FabAction3();
            };


        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutLong("startTime", startTime);



        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);

            if (savedInstanceState != null)
            {
                startlayout.Visibility = ViewStates.Gone;
                fabContainer.Visibility = ViewStates.Visible;
                chronometer.Visibility = ViewStates.Visible;
                startTime = savedInstanceState.GetLong("startTime");
                chronometer.Base = startTime;
                chronometer.Start();
            }
        }


        #region Actions
        public void FabAction1()
        {
            Log.Info(TAG, "Action 1");
        }

        public void FabAction2()
        {
            Log.Info(TAG, "Action 2");
        }

        public void FabAction3()    
        {
            Log.Info(TAG, "Action 3");
        }

        public void StartButtonClick()
        {
            
            startlayout.Visibility = ViewStates.Gone;
            fabContainer.Visibility = ViewStates.Visible;
            chronometer.Visibility = ViewStates.Visible;
         
            startTime = SystemClock.ElapsedRealtime();
            chronometer.Base = startTime;
            chronometer.Start();
        }
        #endregion

        #region Animations
        private void collapseFab()
        {
            fab.SetImageResource(Resource.Drawable.animated_minus);
            AnimatorSet animatorSet = new AnimatorSet();
            animatorSet.PlayTogether(createCollapseAnimator(fabAction1, offset1),
                    createCollapseAnimator(fabAction2, offset2),
                    createCollapseAnimator(fabAction3, offset3));
            animatorSet.Start();
            animateFab();
        }

        private void expandFab()
        {
            fab.SetImageResource(Resource.Drawable.animated_plus);
            AnimatorSet animatorSet = new AnimatorSet();
            animatorSet.PlayTogether(createExpandAnimator(fabAction1, offset1),
                    createExpandAnimator(fabAction2, offset2),
                    createExpandAnimator(fabAction3, offset3));
            animatorSet.Start();
            animateFab();
        }

        private Animator createCollapseAnimator(View view, float offset)
        {
            return ObjectAnimator.OfFloat(view, TRANSLATION_Y, 0, offset)
                    .SetDuration(Resources.GetInteger(Android.Resource.Integer.ConfigMediumAnimTime));
        }

        private Animator createExpandAnimator(View view, float offset)
        {
            return ObjectAnimator.OfFloat(view, TRANSLATION_Y, offset, 0)
                    .SetDuration(Resources.GetInteger(Android.Resource.Integer.ConfigMediumAnimTime));
        }

        private void animateFab()
        {
            Drawable drawable = fab.Drawable;
            if (drawable is IAnimatable)
            {
                ((IAnimatable)drawable).Start();
            }
        }
        #endregion





    }
}